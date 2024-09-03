namespace HealthDiary.API.Model.DTO
{
    public record UserDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Token { get; set; }
    }
}
