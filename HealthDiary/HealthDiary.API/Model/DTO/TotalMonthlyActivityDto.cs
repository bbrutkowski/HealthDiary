namespace HealthDiary.API.Model.DTO
{
    public record TotalMonthlyActivityDto
    {
        public decimal TotalDistance { get; set; }
        public decimal TotalExerciseTime { get; set; }
        public decimal TotalCalorieConsumption { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
