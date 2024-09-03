using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthDiary.API.Model.Main
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(10,3)")]
        public decimal TotalDistance { get; set; }
        [Column(TypeName = "decimal(10,3)")]
        public decimal TotalExerciseTime { get; set; }
        [Column(TypeName = "decimal(10,3)")]
        public decimal TotalCalorieConsumption { get; set; }
        public DateTime CreationDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
