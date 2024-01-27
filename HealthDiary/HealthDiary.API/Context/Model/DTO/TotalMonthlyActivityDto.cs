namespace HealthDiary.API.Context.Model.DTO
{
    public record TotalMonthlyActivityDto
    {
        public decimal TotalDistance { get; set; }
        public decimal TotalExerciseTime { get; set; }
        public decimal TotalCalorieConsumption { get; set; }
    }
}
