using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Monopolizers.Repository.Migrations
{
    public partial class AddUserSavedCardTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSavedCard",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    CardId = table.Column<int>(nullable: false),
                    Background = table.Column<string>(nullable: false),
                    ElementsJson = table.Column<string>(nullable: false),
                    SavedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSavedCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSavedCard_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSavedCard_UserId",
                table: "UserSavedCard",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSavedCard");
        }
    }
}
