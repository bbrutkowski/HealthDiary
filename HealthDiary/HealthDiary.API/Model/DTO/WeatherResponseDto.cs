namespace HealthDiary.API.Model.DTO
{
    public record WeatherResponseDto
    {
        public string Name { get; set; }
        public MainDto Main { get; set; }
        public WeatherDto[] Weather { get; set; }
    }

    public record MainDto
    {
        public float Temp { get; set; }
    }

    public record WeatherDto
    {
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}
