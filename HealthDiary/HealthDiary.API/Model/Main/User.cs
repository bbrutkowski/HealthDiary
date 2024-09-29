using System.ComponentModel.DataAnnotations;

namespace HealthDiary.API.Model.Main
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public Address Address { get; set; } = new Address();
        public List<Weight> Weights { get; set; } = new();
        public List<Sleep> Sleeps { get; set; } = new();
        public List<Activity> Activities { get; set; } = new();
        public List<Food> Foods { get; set; } = new();
        public DateTime BirthDate { get; set; }
        public string? Token { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;
        public WeightGoal WeightGoal { get; set; } = new();
        public decimal Height { get; set; }
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
