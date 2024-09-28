using CSharpFunctionalExtensions;
using HealthDiary.API.Helpers.Interface;
using System.Security.Claims;

namespace HealthDiary.API.Helpers
{
    public class IdentityVerifier : IIdentityVerifier
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private const string UserNotFoundError = "User not found";
        private const string TokenReadError = "Error while reading Id from token";
        private const string IdComparisonError = "Id from the access token is different from the one sent";

        public IdentityVerifier(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        private Result<string> GetUserIdFromToken()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user is null) return Result.Failure<string>(UserNotFoundError);
     
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public Result<bool> IsIdentityConfirmed(int userId)
        {
            var tokenResult = GetUserIdFromToken();
            if (tokenResult.IsFailure || string.IsNullOrEmpty(tokenResult.Value)) return Result.Failure<bool>(TokenReadError);

            if (int.TryParse(tokenResult.Value, out int parsedValue) && userId != parsedValue) return Result.Failure<bool>(IdComparisonError);

            return Result.Success(true);
        }
    }
}
