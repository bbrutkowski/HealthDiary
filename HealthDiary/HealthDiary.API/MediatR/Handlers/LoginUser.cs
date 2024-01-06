using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Context.Model;
using HealthDiary.API.Context.Model.Main;
using HealthDiary.API.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthDiary.API.MediatR.Handlers
{
    public record LoginUserRequest(string UserName, string Password) : IRequest<OperationResult>;

    public class LoginUser : IRequestHandler<LoginUserRequest, OperationResult>
    {
        private readonly DataContext _context;

        private const string UserNotFoundError = "User not found";
        private const string UserCredentialsError = "User name or password not valid";

        public LoginUser(DataContext context) => _context = context;

        public async Task<OperationResult> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.IsActive && x.Name == request.UserName, cancellationToken);
            if (user is null) return OperationResultExtensions.Failure(UserNotFoundError);

            if (!PasswordHasher.Verify(request.Password, user.Password)) return OperationResultExtensions.Failure(UserCredentialsError);

            user.Token = CreateJwtToken(user);

            return OperationResultExtensions.Success(user);
        }

        private static string CreateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF32.GetBytes("applicationKey");

            var identity = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Role, user.Role.ToString()),
                new(ClaimTypes.Name, user.Name)
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
