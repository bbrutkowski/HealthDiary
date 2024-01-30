using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthDiary.API.Context.Model.Main
{
    public class Food
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal Protein { get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal Fat {  get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal Carbohydrates { get; set; }
        public int Kcal {  get; set; }
        public DateTime CreationDate { get; set; }
        public int UserId { get; set;}
        public User User { get; set; }
    }
}
