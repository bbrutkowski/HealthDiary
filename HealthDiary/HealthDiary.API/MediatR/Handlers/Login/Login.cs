using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Helpers;
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
            private const string UserCredentialsError = "User name or password not valid";

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
                    .AsNoTracking()
                    .Where(x => x.IsActive && x.Login == request.Login)
                    .Select(x => new UserAlias
                    {
                        Id = x.Id,
                        Login = x.Login,
                        Password = x.Password,
                        Role = x.Role                      
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (user is null) return Result.Failure<UserDto>(UserNotFoundError);

                if (!_passwordHasher.Verify(request.Password, user.Password)) return Result.Failure<UserDto>(UserCredentialsError);

                user.Token = CreateJwtToken(user);

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Name = user.Login,
                    Token = user.Token,
                    Role = user.Role
                };

                return Result.Success(userDto);
            }

            private string CreateJwtToken(UserAlias user)
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

                return new JwtSecurityTokenHandler().WriteToken(token);
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
