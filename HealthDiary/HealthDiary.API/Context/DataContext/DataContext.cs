﻿using HealthDiary.API.Context.Model.Main;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.Context.DataContext
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<WeatherInfoBar> WeatherInformations { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Weight> Weights { get; set; }
        public DbSet<Sleep> Sleeps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId);

            modelBuilder.Entity<Weight>()
                .HasOne(w => w.User)
                .WithMany(u => u.Weights)
                .HasForeignKey(w => w.UserId);

            modelBuilder.Entity<Sleep>()
                .HasOne(s => s.User)
                .WithMany(u => u.Sleeps)
                .HasForeignKey(s => s.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
