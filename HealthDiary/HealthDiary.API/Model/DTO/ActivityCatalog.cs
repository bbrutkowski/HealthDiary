namespace HealthDiary.API.Model.DTO
{
    public record ActivityCatalog
    {
        public List<ActivityDto> Activities { get; set; }
        public decimal LastUserWeight { get; set; }
    }
}
