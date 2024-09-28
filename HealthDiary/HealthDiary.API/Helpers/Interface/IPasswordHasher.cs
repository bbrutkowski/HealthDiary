using CSharpFunctionalExtensions;

namespace HealthDiary.API.Helpers.Interface
{
    public interface IPasswordHasher
    {
        public Result<string> Hash(string password);
        public Result<bool> Verify(string password, string base64Hash);
    }
}
