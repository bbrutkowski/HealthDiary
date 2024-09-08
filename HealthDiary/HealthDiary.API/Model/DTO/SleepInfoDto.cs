namespace HealthDiary.API.Model.DTO
{
    public record SleepInfoDto
    {
        public decimal SleepTime { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
