using CSharpFunctionalExtensions;
using HealthDiary.API.Context;
using HealthDiary.API.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers
{
    public record LoginUserRequest : IRequest<Result>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class LoginUser : IRequestHandler<LoginUserRequest, Result>
    {
        private readonly DataContext _context;

        private const string UserNotFoundError = "User not found";
        private const string UserCredentialsError = "User name or password not valid";

        public LoginUser(DataContext context) => _context = context;

        public async Task<Result> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.IsActive && x.Name == request.UserName, cancellationToken);
            if (user is null) return Result.Failure(UserNotFoundError);

            if (!PasswordHasher.Verify(request.Password, user.Password)) return Result.Failure(UserCredentialsError);

            return Result.Success(user);
        }
    }
}
