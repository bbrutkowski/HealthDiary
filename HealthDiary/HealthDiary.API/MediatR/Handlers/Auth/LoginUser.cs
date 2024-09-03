using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Helpers;
using HealthDiary.API.Model.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserAlias = HealthDiary.API.Model.Main.User;

namespace HealthDiary.API.MediatR.Handlers.Auth
{
    public static class LoginUser
    {
        public record LoginRequest(string Login, string Password) : IRequest<Result<UserDto>>;

        public sealed class Handler : IRequestHandler<LoginRequest, Result<UserDto>>
        {
            private readonly DataContext _context;
            private readonly IValidator<LoginRequest> _requestValidator;

            private const string UserNotFoundError = "User not found";
            private const string UserCredentialsError = "User name or password not valid";

            public Handler(DataContext dataContext, IValidator<LoginRequest> validator)
            {
                _context = dataContext;
                _requestValidator = validator;
            }

            public async Task<Result<UserDto>> Handle(LoginRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return Result.Failure<UserDto>(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var user = await _context.Users
                    .AsNoTracking()
                    .Where(x => x.IsActive && x.Login == request.Login)
                    .Select(x => new UserAlias { Id = x.Id, Login = x.Login, Password = x.Password })
                    .FirstOrDefaultAsync(cancellationToken);

                if (user is null) return Result.Failure<UserDto>(UserNotFoundError);

                if (!PasswordHasher.Verify(request.Password, user.Password)) return Result.Failure<UserDto>(UserCredentialsError);

                user.Token = CreateJwtToken(user);

                return Result.Success(new UserDto { Id = user.Id, Name = user.Login, Token = user.Token });
            }

            private static string CreateJwtToken(UserAlias user)
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF32.GetBytes("applicationKey");
 
                var identity = new ClaimsIdentity(new Claim[]
                {
                   new(ClaimTypes.Role, user.Role.ToString()),
                   new(ClaimTypes.Name, user.Login)
                });

                var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = identity,
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = credentials
                };

                var token = jwtTokenHandler.CreateToken(tokenDescriptor);
                return jwtTokenHandler.WriteToken(token);
            }
        }

        public sealed class Validator : AbstractValidator<LoginRequest>
        {
            public const string LoginValidationError = "Login is null or empty";
            public const string PasswordValidationError = "Password is null or empty";

            public Validator()
            {
                RuleFor(x => x.Login)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage(LoginValidationError);

                RuleFor(x => x.Password)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage(PasswordValidationError);
            }
        }
    }
}
