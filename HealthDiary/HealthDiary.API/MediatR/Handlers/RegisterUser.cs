﻿using CSharpFunctionalExtensions;
using HealthDiary.API.Context;
using HealthDiary.API.Context.Model;
using HealthDiary.API.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers
{
    public record RegisterUserRequest(string Name, string Password, string Email) : IRequest<Result>;
   
    public class RegisterUser : IRequestHandler<RegisterUserRequest, Result>
    {
        private readonly DataContext _context;

        private const string UserNameValidationError = "User name already exists";
        private const string EmailValidationError = "Email adress is taken";

        public RegisterUser(DataContext context) => _context = context;

        public async Task<Result> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                Name = request.Name,
                Email = request.Email,
                Password = PasswordHasher.Hash(request.Password)
            };

            try
            {
                var validationResult = await ValidateUser(user, cancellationToken);
                if (validationResult.ToString() != string.Empty) return Result.Failure(validationResult.ToString());

                await _context.Users.AddAsync(user, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);
            }
        }

        private async Task<string> ValidateUser(User newUser, CancellationToken cancellationToken)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.IsActive && x.Name == newUser.Name && x.Email == newUser.Email, cancellationToken);

            if (existingUser is not null) 
            {
                if (existingUser.Name == newUser.Name) return UserNameValidationError;
                if (existingUser.Email == newUser.Email) return EmailValidationError;              
            }

            return string.Empty;
        }
    }
}
