using System;

namespace HealthDiary.Database.Model.Infrastructure.Basic
{
    public class BasicEntity
    {
        private const int DefaultCreationBy = 1;
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; } = true;

        public BasicEntity()
        {
            CreationDate = DateTime.Now;
            CreatedBy = DefaultCreationBy;
        }
    }
}
