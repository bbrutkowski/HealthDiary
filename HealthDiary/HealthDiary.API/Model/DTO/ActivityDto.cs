namespace HealthDiary.API.Model.DTO
{
    public record ActivityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal MET { get; set; }
        public decimal AverageSpeed { get; set; }
    }
}
