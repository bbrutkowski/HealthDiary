using System.ComponentModel.DataAnnotations;

namespace HealthDiary.API.Context.Model.Main
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TotalDistance { get; set; }
        public decimal TotalExerciseTime { get; set; }  
        public decimal TotalCalorieConsumption { get; set; }
        public DateTime CreationDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
