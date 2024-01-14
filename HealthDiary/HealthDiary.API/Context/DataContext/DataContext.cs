using HealthDiary.API.Context.Model.Main;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.Context.DataContext
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<WeatherInfoBar> WeatherInformations { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable(nameof(Users));
            modelBuilder.Entity<WeatherInfoBar>().ToTable(nameof(WeatherInfoBar));
            modelBuilder.Entity<Address>().ToTable(nameof(Addresses));

            modelBuilder.Entity<User>()
                        .HasOne(u => u.Address)
                        .WithOne(a => a.User)
                        .HasForeignKey<Address>(a => a.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
