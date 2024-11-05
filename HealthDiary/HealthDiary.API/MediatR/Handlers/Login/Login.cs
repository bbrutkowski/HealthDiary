using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Helpers.Interface;
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
    public static class Login
    {
        public record LoginRequest(string Login, string Password) : IRequest<Result<UserDto>>;

        public sealed class Handler : IRequestHandler<LoginRequest, Result<UserDto>>
        {
            private readonly DataContext _context;
            private readonly IValidator<LoginRequest> _requestValidator;
            private readonly IConfiguration _configuration;
            private readonly IPasswordHasher _passwordHasher;

            private const string UserNotFoundError = "User not found";

            public Handler(
                DataContext dataContext,
                IValidator<LoginRequest> validator,
                IConfiguration configuration,
                IPasswordHasher passwordHasher)
            {
                _context = dataContext;
                _requestValidator = validator;
                _configuration = configuration;
                _passwordHasher = passwordHasher;
            }

            public async Task<Result<UserDto>> Handle(LoginRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return Result.Failure<UserDto>(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var user = await _context.Users
                    .FirstAsync(x => x.Login == request.Login);

                if (user is null) return Result.Failure<UserDto>(UserNotFoundError);

                var passwordVerificatioResult = _passwordHasher.Verify(request.Password, user.Password);
                if (passwordVerificatioResult.IsFailure) return Result.Failure<UserDto>(passwordVerificatioResult.Error);

                var accessTokenResult = CreateJwtToken(user);
                if (accessTokenResult.IsFailure) return Result.Failure<UserDto>(accessTokenResult.Error);

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Name = user.Login,
                    Token = accessTokenResult.Value,
                    Role = user.Role
                };

                return Result.Success(userDto);
            }

            private Result<string> CreateJwtToken(UserAlias user)
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var claims = new[]
                {
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Login)
                };

                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                   issuer: _configuration["Jwt:Issuer"],
                   audience: _configuration["Jwt:Audience"],
                   claims: claims,
                   expires: DateTime.Now.AddMinutes(30),
                   signingCredentials: credentials);

                var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

                return Result.Success(accessToken);
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
