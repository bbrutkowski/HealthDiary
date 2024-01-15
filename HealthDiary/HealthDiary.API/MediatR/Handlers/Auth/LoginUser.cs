using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Context.Model;
using HealthDiary.API.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserAlias = HealthDiary.API.Context.Model.Main.User;

namespace HealthDiary.API.MediatR.Handlers.Auth
{
    public static class LoginUser
    {
        public record LoginUserRequest(string Login, string Password) : IRequest<OperationResult>;

        public sealed class Handler : IRequestHandler<LoginUserRequest, OperationResult>
        {
            private readonly DataContext _context;
            public Handler(DataContext dataContext) => _context = dataContext;

            private const string UserNotFoundError = "User not found";
            private const string UserCredentialsError = "User name or password not valid";
            public async Task<OperationResult> Handle(LoginUserRequest request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Where(x => x.IsActive && x.Login == request.Login)
                                   .Select(x => new UserAlias { Id = x.Id, Login = x.Login, Password = x.Password })
                                   .FirstOrDefaultAsync(cancellationToken);

                if (user is null) return OperationResultExtensions.Failure(UserNotFoundError);

                if (!PasswordHasher.Verify(request.Password, user.Password)) return OperationResultExtensions.Failure(UserCredentialsError);

                user.Token = CreateJwtToken(user);

                return OperationResultExtensions.Success(user);
            }

            private string CreateJwtToken(UserAlias user)
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
    }
}
