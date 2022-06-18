using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace datingAppBackend.Migrations
{
    public partial class _3893892 : Migration
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
                    isAcepted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SexualOrientations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    modoFantasma = table.Column<bool>(type: "bit", nullable: false),
                    instagramUserEnabled = table.Column<bool>(type: "bit", nullable: false),
                    instagramUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    whatsappNumberEnabled = table.Column<bool>(type: "bit", nullable: false),
                    whatsappNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    minimunAgeToMatch = table.Column<int>(type: "int", nullable: false),
                    maximunAgeToMatch = table.Column<int>(type: "int", nullable: false),
                    deletedAccount = table.Column<bool>(type: "bit", nullable: false),
                    loginStatus = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sexualOrientationId = table.Column<int>(type: "int", nullable: false),
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
                name: "SexualOrientations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
