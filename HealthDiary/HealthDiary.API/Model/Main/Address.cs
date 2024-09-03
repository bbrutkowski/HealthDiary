using System.ComponentModel.DataAnnotations;

namespace HealthDiary.API.Model.Main
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public int BuildingNumber { get; set; }
        public int ApartmentNumber { get; set; }
        public string? PostalCode { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
