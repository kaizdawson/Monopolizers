using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Monopolizers.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Fix_UserSavedCardSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSavedCards_AspNetUsers_UserId",
                table: "UserSavedCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSavedCards",
                table: "UserSavedCards");

            migrationBuilder.RenameTable(
                name: "UserSavedCards",
                newName: "UserSavedCard");

            migrationBuilder.RenameIndex(
                name: "IX_UserSavedCards_UserId",
                table: "UserSavedCard",
                newName: "IX_UserSavedCard_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSavedCard",
                table: "UserSavedCard",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavedCard_AspNetUsers_UserId",
                table: "UserSavedCard",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSavedCard_AspNetUsers_UserId",
                table: "UserSavedCard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSavedCard",
                table: "UserSavedCard");

            migrationBuilder.RenameTable(
                name: "UserSavedCard",
                newName: "UserSavedCards");

            migrationBuilder.RenameIndex(
                name: "IX_UserSavedCard_UserId",
                table: "UserSavedCards",
                newName: "IX_UserSavedCards_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSavedCards",
                table: "UserSavedCards",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavedCards_AspNetUsers_UserId",
                table: "UserSavedCards",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
