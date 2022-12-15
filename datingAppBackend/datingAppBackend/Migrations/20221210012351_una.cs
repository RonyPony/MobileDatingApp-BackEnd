using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace datingAppBackend.Migrations
{
    public partial class una : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    enabled = table.Column<bool>(type: "bit", nullable: false),
                    createdOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    originUserId = table.Column<int>(type: "int", nullable: false),
                    finalUserId = table.Column<int>(type: "int", nullable: false),
                    isAcepted = table.Column<bool>(type: "bit", nullable: false),
                    isSeen = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SexualOrientations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    imageId = table.Column<int>(type: "int", nullable: false),
                    enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SexualOrientations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    countryId = table.Column<int>(type: "int", nullable: false),
                    isEnabled = table.Column<bool>(type: "bit", nullable: false),
                    preferedCountryId = table.Column<int>(type: "int", nullable: false),
                    modoFantasma = table.Column<bool>(type: "bit", nullable: false),
                    instagramUserEnabled = table.Column<bool>(type: "bit", nullable: false),
                    instagramUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    whatsappNumberEnabled = table.Column<bool>(type: "bit", nullable: false),
                    whatsappNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bornDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    showMySexuality = table.Column<bool>(type: "bit", nullable: false),
                    minimunAgeToMatch = table.Column<int>(type: "int", nullable: false),
                    maximunAgeToMatch = table.Column<int>(type: "int", nullable: false),
                    deletedAccount = table.Column<bool>(type: "bit", nullable: false),
                    loginStatus = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sexualOrientationId = table.Column<int>(type: "int", nullable: false),
                    sex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sexualPreferenceId = table.Column<int>(type: "int", nullable: false),
                    registerDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lastLogin = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "SexualOrientations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
