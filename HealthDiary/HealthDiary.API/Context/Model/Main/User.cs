using System.ComponentModel.DataAnnotations;

namespace HealthDiary.API.Context.Model.Main
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int Age { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender? Gender { get; set; }
        public Address Address { get; set; }
        public List<Weight> Weights { get; set; }
        public DateTime? BirthDate { get; set; }
        public double Weight { get; set; }
        public string? Token { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public enum UserRole
    {
        User,
        Admin
    }

    public enum Gender
    {
        Male,
        Female
    }
}
