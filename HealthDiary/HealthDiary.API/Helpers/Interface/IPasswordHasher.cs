namespace HealthDiary.API.Helpers.Interface
{
    public interface IPasswordHasher
    {
        public string Hash(string password);
        public bool Verify(string password, string base64Hash);
    }
}
