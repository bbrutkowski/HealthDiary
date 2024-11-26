namespace HealthDiary.API.Model.DTO
{
    public record WeatherSettingsDto
    {
        public string WeatherUrl { get; set; }
        public string WeatherApiKey { get; set; }
    }
}
