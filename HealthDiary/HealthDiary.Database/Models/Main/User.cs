using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthDiary.BusinessLogic.Models.Main
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Token { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;
    }
    public enum UserRole
    {
        User,
        Admin
    }
}
