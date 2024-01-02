using HealthDiary.BusinessLogic.Models.Main;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.Database
{
    public class DataContext : DbContext
    {
        public DataContext(): base() { }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=HealthDiaryDB;Integrated Security=True;");
        }
    }
}
