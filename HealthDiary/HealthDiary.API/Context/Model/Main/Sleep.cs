using System.ComponentModel.DataAnnotations;

namespace HealthDiary.API.Context.Model.Main
{
    public class Sleep
    {
        [Key]
        public int Id { get; set; }
        public decimal SleepTime { get; set; }
        public DateTime CreationDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
