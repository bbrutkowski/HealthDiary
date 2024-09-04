namespace HealthDiary.API.Model.DTO
{
    public record MealDto
    {
        public string? Name { get; set; }
        public decimal Protein { get; set; }
        public decimal Fat { get; set; }
        public decimal Carbohydrates { get; set; }
        public int Kcal { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
