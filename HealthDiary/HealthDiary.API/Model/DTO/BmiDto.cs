namespace HealthDiary.API.Model.DTO
{
    public record BmiDto
    {
        public decimal Value { get; set; }
        public string? Description { get; set; }
        public string? IndexColor { get; set; }
    }
}
