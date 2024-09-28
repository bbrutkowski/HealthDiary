using CSharpFunctionalExtensions;
using HealthDiary.API.Helpers.Interface;
using System.Security.Cryptography;

namespace HealthDiary.API.Helpers
{
    public class PasswordHasher : IPasswordHasher
    {
        private static readonly int SaltSize = 16;
        private static readonly int HashSize = 20;
        private static readonly int Iterations = 10000;

        private const string VerificationError = "Error verifying password";

        public Result<string> Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var key = new Rfc2898DeriveBytes(password, salt, Iterations);
            var hash = key.GetBytes(HashSize);

            var hashBytes = new byte[HashSize + SaltSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            return Result.Success(Convert.ToBase64String(hashBytes));
        }

        public Result<bool> Verify(string password, string base64Hash)
        {
            var hashBytes = Convert.FromBase64String(base64Hash);

            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            var key = new Rfc2898DeriveBytes(password, salt, Iterations);
            var hash = key.GetBytes(HashSize);

            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i]) return Result.Failure<bool>(VerificationError);
            }

            return Result.Success(true);
        }
    }
}
