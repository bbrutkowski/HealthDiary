namespace HealthDiary.API.Model.DTO
{
    public record GeoSettingsDto
    {
        public string GeoUrl { get; set; }
        public string GeoApiKey { get; set; }
    }
}
