namespace HealthDiary.API.Model.DTO
{
    public record WeeklyNutritionDto
    {
        public int Kcal { get; set; }
        public decimal Protein { get; set; }
        public decimal Fat { get; set; }
        public decimal Carbohydrates { get; set; }
    }
}
