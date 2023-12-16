using HealthDiary.Database.Model.Main;
using System;
using System.Data.Entity;

namespace HealthDiary.Database
{
    public class DataContext : DbContext
    {
        public DataContext(): base("MainModel_HealthDiary") { }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<string>()
                .Configure(x => x.HasColumnType("nvarchar")
                .HasMaxLength(255));

            modelBuilder.Properties<DateTime>()
                .Configure(x => x.HasColumnType("datetime2")
                .HasPrecision(7));

            modelBuilder.Properties<decimal>()
                .Configure(x => x.HasPrecision(5, 2));


            base.OnModelCreating(modelBuilder);
        }
    }
}
