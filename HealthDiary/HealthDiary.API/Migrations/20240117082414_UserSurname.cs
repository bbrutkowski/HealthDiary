using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthDiary.API.Migrations
{
    public partial class UserSurname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Surname",
                table: "Users");
        }
    }
}
