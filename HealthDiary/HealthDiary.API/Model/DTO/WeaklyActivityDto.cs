namespace HealthDiary.API.Model.DTO
{
    public record WeaklyActivityDto
    {
        public string WeekRange { get; set; }
        public int Year { get; set; }
        public decimal TotalCalorieConsumption { get; set; }
        public decimal TotalDistance { get; set; }
        public decimal TotalExerciseTime { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
