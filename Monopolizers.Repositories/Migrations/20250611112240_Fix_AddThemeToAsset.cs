using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Monopolizers.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Fix_AddThemeToAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
             name: "Theme",
             table: "Asset",
             type: "nvarchar(max)",
             nullable: false,
             defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
             name: "Theme",
             table: "Asset");
        }
    }
}
