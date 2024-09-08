namespace HealthDiary.API.Model.DTO
{
    public record WeightDto
    {
        public decimal Value { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
