using CSharpFunctionalExtensions;

namespace HealthDiary.API.Helpers.Interface
{
    public interface IIdentityVerifier
    {
        public Result<bool> IsIdentityConfirmed(int userId);
    }
}
