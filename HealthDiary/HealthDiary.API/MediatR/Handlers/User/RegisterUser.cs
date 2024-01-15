﻿using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Context.Model;
using HealthDiary.API.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UserAlias = HealthDiary.API.Context.Model.Main.User;

namespace HealthDiary.API.MediatR.Handlers.User
{
    public static class RegisterUser
    {
        public record RegisterUserRequest(string Login, string Password, string Email) : IRequest<OperationResult>;

        public sealed class Handler : IRequestHandler<RegisterUserRequest, OperationResult>
        {
            private readonly DataContext _context;

            public Handler(DataContext dataContext) => _context = dataContext;

            private const string UserNameValidationError = "User name already exists";
            private const string EmailValidationError = "Email adress is taken";

            public async Task<OperationResult> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
            {
                var user = new UserAlias()
                {
                    Login = request.Login,
                    Email = request.Email,
                    Password = PasswordHasher.Hash(request.Password)
                };

                try
                {
                    var validationResult = await ValidateUser(user, cancellationToken);
                    if (validationResult.ToString() != string.Empty) return OperationResultExtensions.Failure(validationResult.ToString());

                    await _context.Users.AddAsync(user, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);

                    return OperationResultExtensions.Success();
                }
                catch (Exception e)
                {
                    return OperationResultExtensions.Failure(e.Message);
                }
            }

            private async Task<string> ValidateUser(UserAlias newUser, CancellationToken cancellationToken)
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.IsActive && x.Login == newUser.Login || x.Email == newUser.Email, cancellationToken);

                if (existingUser is not null)
                {
                    if (existingUser.Login == newUser.Login) return UserNameValidationError;
                    if (existingUser.Email == newUser.Email) return EmailValidationError;
                }

                return string.Empty;
            }
        }
    }   
}
