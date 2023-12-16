namespace HealthDiary.Database.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataContext context)
        {
            context.Users.Add(new Model.Main.User { Name = "admin", Password = "admin", Email = "admin@admin.pl" });
        }
    }
}
