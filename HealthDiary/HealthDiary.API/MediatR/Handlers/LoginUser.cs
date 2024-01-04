using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Context.Model;
using HealthDiary.API.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

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

            return OperationResultExtensions.Success(user);
        }
    }
}
