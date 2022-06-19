using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace datingAppBackend.Migrations
{
    public partial class _29883 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "preferedCountryId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "preferedCountryId",
                table: "Users");
        }
    }
}
