using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Helpers;
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

            private const string UserNameValidationError = "User name already exists";
            private const string EmailValidationError = "Email adress is taken";
            private const string RegisterUserError = "Error during user registration";
            private const string UserValidationSuccessful = "User does not exist. Registration possible";

            public Handler(DataContext dataContext, IValidator<RegisterUserRequest> validator)
            {
                _context = dataContext;
                _requestValidator = validator;
            }

            public async Task<Result<bool>> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return Result.Failure<bool>(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var validationResult = await CheckUserCredentialsAsync(request.Login, request.Email, cancellationToken);
                if (validationResult != UserValidationSuccessful) return Result.Failure<bool>(validationResult.ToString());

                var user = new UserAlias()
                {
                    Login = request.Login,
                    Email = request.Email,
                    Password = PasswordHasher.Hash(request.Password)
                };

                await _context.Users.AddAsync(user, cancellationToken);
                var changes = await _context.SaveChangesAsync(cancellationToken);

                return changes > 0 ? Result.Success(true) : Result.Failure<bool>(RegisterUserError);
            }

            private async Task<string> CheckUserCredentialsAsync(string login, string email, CancellationToken cancellationToken)
            {
                var existingUser = await _context.Users
                    .AsNoTracking()
                    .Where(x => x.IsActive && x.Login == login || x.Email == email)
                    .FirstOrDefaultAsync(cancellationToken);

                if (existingUser is not null)
                {
                    if (existingUser.Login == login) return UserNameValidationError;
                    if (existingUser.Email == email) return EmailValidationError;
                }

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
