using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Monopolizers.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderCodeToWalletTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderCode",
                table: "WalletTransaction",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderCode",
                table: "WalletTransaction");


        }
    }
}
