using System.ComponentModel.DataAnnotations;

namespace HealthDiary.API.Context.Model.Main
{
    public class WeatherInfoBar
    {
        [Key]
        public int Id { get; set; }
        public string? Content { get; set; }
        public bool IsActive { get; set; }
    }
}
