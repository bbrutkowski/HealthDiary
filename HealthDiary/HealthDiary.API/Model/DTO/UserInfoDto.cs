using HealthDiary.API.Model.Main;

namespace HealthDiary.API.Model.DTO
{
    public record UserInfoDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public AddressDto Address { get; set; }
    }
}
