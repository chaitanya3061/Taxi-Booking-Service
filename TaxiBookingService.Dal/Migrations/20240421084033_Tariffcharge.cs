using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaxiBookingService.Dal.Migrations
{
    /// <inheritdoc />
    public partial class Tariffcharge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TariffCharge",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "Cancellation Fee", 5m },
                    { 2, "Per Km", 3.20m },
                    { 3, "Base fare", 32.0m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TariffCharge",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TariffCharge",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TariffCharge",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
