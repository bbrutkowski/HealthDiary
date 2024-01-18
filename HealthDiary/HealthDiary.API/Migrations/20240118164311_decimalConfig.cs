using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthDiary.API.Migrations
{
    public partial class decimalConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WeatherInfoBar",
                table: "WeatherInfoBar");

            migrationBuilder.RenameTable(
                name: "WeatherInfoBar",
                newName: "WeatherInformations");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Weights",
                type: "decimal(4,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeatherInformations",
                table: "WeatherInformations",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WeatherInformations",
                table: "WeatherInformations");

            migrationBuilder.RenameTable(
                name: "WeatherInformations",
                newName: "WeatherInfoBar");

            migrationBuilder.AlterColumn<double>(
                name: "Value",
                table: "Weights",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeatherInfoBar",
                table: "WeatherInfoBar",
                column: "Id");
        }
    }
}
