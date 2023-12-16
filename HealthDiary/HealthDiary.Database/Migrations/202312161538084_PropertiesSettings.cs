namespace HealthDiary.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PropertiesSettings : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.Users", "Password", c => c.String(maxLength: 255));
            AlterColumn("dbo.Users", "Email", c => c.String(maxLength: 255));
            AlterColumn("dbo.Users", "BirthDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Users", "BasicEntity_CreationDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "BasicEntity_CreationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "BirthDate", c => c.DateTime());
            AlterColumn("dbo.Users", "Email", c => c.String());
            AlterColumn("dbo.Users", "Password", c => c.String());
            AlterColumn("dbo.Users", "Name", c => c.String());
        }
    }
}
