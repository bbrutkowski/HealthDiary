using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UserAlias = HealthDiary.API.Context.Model.Main.User;

namespace HealthDiary.API.MediatR.Handlers.User
{
    public static class RegisterUser
    {
        public record RegisterUserRequest(string Login, string Password, string Email) : IRequest<Result>;

        public sealed class Handler : IRequestHandler<RegisterUserRequest, Result>
        {
            private readonly DataContext _context;
            private readonly IValidator<RegisterUserRequest> _requestValidator;

            private const string UserNameValidationError = "User name already exists";
            private const string EmailValidationError = "Email adress is taken";

            public Handler(DataContext dataContext, IValidator<RegisterUserRequest> validator)
            {
                _context = dataContext;
                _requestValidator = validator;
            }

            public async Task<Result> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return Result.Failure(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var user = new UserAlias()
                {
                    Login = request.Login,
                    Email = request.Email,
                    Password = PasswordHasher.Hash(request.Password)
                };

                try
                {
                    var validationResult = await CheckUserCredentialsAsync(user, cancellationToken);
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

            private async Task<string> CheckUserCredentialsAsync(UserAlias newUser, CancellationToken cancellationToken)
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

        public sealed class Validator : AbstractValidator<RegisterUserRequest>
        {
            public const string UserLoginValidation = "User login is null or empty";
            public const string PasswordValidation = "Password is null or empty";
            public const string EmailValidation = "Email is null or empty";
           
            public Validator()
            {
                RuleFor(x => x.Login).NotEmpty().NotNull().WithMessage(UserLoginValidation);
                RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage(PasswordValidation);
                RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage(EmailValidation);
            }
        }
    }   
}
