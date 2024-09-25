namespace HealthDiary.API.Model.DTO
{
    public record WeightGoalDto
    {
        public bool IsSet { get; set; }
        public decimal CurrentWeight { get; set; }
        public decimal TargetWeight { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime TargetDate { get; set; }
    }
}
