using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxiBookingService.Dal.Migrations
{
    /// <inheritdoc />
    public partial class PaymentNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RideId",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_RideId",
                table: "Payment",
                column: "RideId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Ride_RideId",
                table: "Payment",
                column: "RideId",
                principalTable: "Ride",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Ride_RideId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_RideId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "RideId",
                table: "Payment");
        }
    }
}
