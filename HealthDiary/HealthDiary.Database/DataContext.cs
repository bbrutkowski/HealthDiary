using HealthDiary.Database.Model.Main;
using System.Data.Entity;

namespace HealthDiary.Database
{
    public class DataContext : DbContext
    {
        public DataContext(): base("MainModel_HealthDiary") { }
        public DbSet<User> Users { get; set; }
    }
}
