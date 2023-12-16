using HealthDiary.Database.Model.Infrastructure.Basic;
using HealthDiary.Database.Model.Infrastructure.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace HealthDiary.Database.Model.Main
{
    public class User : IBasicEntity, IIdenticableEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public BasicEntity BasicEntity { get; set; } = new BasicEntity();
        public bool IsNew() => Id == 0; 
    }
}
