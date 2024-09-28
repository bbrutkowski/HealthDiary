using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Helpers;
using HealthDiary.API.Helpers.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UserAlias = HealthDiary.API.Model.Main.User;

namespace HealthDiary.API.MediatR.Handlers.User
{
    public static class RegisterUser
    {
        public record RegisterUserRequest(string Login, string Password, string Email) : IRequest<Result<bool>>;

        public sealed class Handler : IRequestHandler<RegisterUserRequest, Result<bool>>
        {
            private readonly DataContext _context;
            private readonly IValidator<RegisterUserRequest> _requestValidator;
            private readonly IPasswordHasher _passwordHasher;

            private const string UserNameValidationError = "User name already exists";
            private const string EmailValidationError = "Email adress is taken";
            private const string RegisterUserError = "Error during user registration";
            private const string UserValidationSuccessful = "User does not exist. Registration possible";

            public Handler(DataContext dataContext,
                           IValidator<RegisterUserRequest> validator,
                           IPasswordHasher passwordHasher)
            {
                _context = dataContext;
                _requestValidator = validator;
                _passwordHasher = passwordHasher;
            }

            public async Task<Result<bool>> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return Result.Failure<bool>(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var validationResult = await CheckUserCredentialsAsync(request.Login, request.Email, cancellationToken);
                if (validationResult.IsFailure) return Result.Failure<bool>(validationResult.Error);

                var hashedPasswordResult = _passwordHasher.Hash(request.Password);
                if (hashedPasswordResult.IsFailure) return Result.Failure<bool>(hashedPasswordResult.Error);

                var user = new UserAlias()
                {
                    Login = request.Login,
                    Email = request.Email,
                    Role = Model.Main.UserRole.User,
                    Password = hashedPasswordResult.Value
                };

                await _context.Users.AddAsync(user, cancellationToken);
                var changes = await _context.SaveChangesAsync(cancellationToken);

                return changes > 0 ? Result.Success(true) : Result.Failure<bool>(RegisterUserError);
            }

            private async Task<Result<string>> CheckUserCredentialsAsync(string login, string email, CancellationToken cancellationToken)
            {
                var existingUser = await _context.Users
                    .AsNoTracking()
                    .Where(x => x.IsActive && x.Login == login || x.Email == email)
                    .FirstOrDefaultAsync(cancellationToken);

                if (existingUser is null) return Result.Failure<string>(UserValidationSuccessful);

                if (existingUser.Login == login) return UserNameValidationError;
                if (existingUser.Email == email) return EmailValidationError;

                return UserValidationSuccessful;
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
