using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxiBookingService.Dal.Migrations
{
    /// <inheritdoc />
    public partial class TariffchargeChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TariffCharge",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "CancellationFee");

            migrationBuilder.UpdateData(
                table: "TariffCharge",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "PerKm");

            migrationBuilder.UpdateData(
                table: "TariffCharge",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Basefare");

            migrationBuilder.InsertData(
                table: "TariffCharge",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[] { 4, "driverCommissionRate", 10m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TariffCharge",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "TariffCharge",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Cancellation Fee");

            migrationBuilder.UpdateData(
                table: "TariffCharge",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Per Km");

            migrationBuilder.UpdateData(
                table: "TariffCharge",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Base fare");
        }
    }
}
