using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthDiary.API.Model.Main
{
    public class Weight
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal Value { get; set; }
        public DateTime CreationDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
