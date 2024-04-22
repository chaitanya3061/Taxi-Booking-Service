using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxiBookingService.Dal.Migrations
{
    /// <inheritdoc />
    public partial class Firs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User");

            migrationBuilder.UpdateData(
                table: "RideStatus",
                keyColumn: "Id",
                keyValue: 4,
                column: "Status",
                value: "Completed");

            migrationBuilder.UpdateData(
                table: "RideStatus",
                keyColumn: "Id",
                keyValue: 5,
                column: "Status",
                value: "Cancelled");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User");

            migrationBuilder.UpdateData(
                table: "RideStatus",
                keyColumn: "Id",
                keyValue: 4,
                column: "Status",
                value: "Started");

            migrationBuilder.UpdateData(
                table: "RideStatus",
                keyColumn: "Id",
                keyValue: 5,
                column: "Status",
                value: "Started");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
