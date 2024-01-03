using System.ComponentModel.DataAnnotations;

namespace HealthDiary.API.Context.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Token { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public enum UserRole
    {
        User,
        Admin
    }
}
