using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoApp.Migrations
{
    /// <inheritdoc />
    public partial class updatePriceHistoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "PriceHistories",
                newName: "NewPrice");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "PriceHistories",
                newName: "Timestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "PriceHistories",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "NewPrice",
                table: "PriceHistories",
                newName: "Price");
        }
    }
}
