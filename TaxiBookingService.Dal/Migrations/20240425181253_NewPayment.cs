using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxiBookingService.Dal.Migrations
{
    /// <inheritdoc />
    public partial class NewPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payment_RideId",
                table: "Payment");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2024, 4, 25, 18, 12, 52, 370, DateTimeKind.Utc).AddTicks(7352), new byte[] { 155, 16, 185, 148, 156, 41, 120, 121, 80, 204, 30, 233, 158, 89, 94, 68, 108, 111, 218, 155, 69, 119, 185, 17, 44, 149, 177, 234, 173, 14, 101, 44, 182, 85, 107, 168, 115, 28, 139, 82, 228, 251, 96, 25, 15, 137, 189, 84, 121, 165, 166, 201, 143, 251, 2, 86, 224, 239, 11, 46, 151, 229, 239, 215 }, new byte[] { 223, 179, 25, 248, 104, 137, 197, 169, 82, 199, 144, 6, 139, 166, 47, 160, 181, 53, 207, 170, 134, 153, 192, 13, 27, 138, 241, 103, 218, 208, 58, 25, 40, 221, 26, 5, 89, 149, 135, 218, 107, 102, 238, 133, 125, 176, 25, 202, 113, 164, 233, 48, 217, 173, 68, 83, 133, 58, 176, 86, 214, 93, 29, 193, 138, 148, 200, 19, 101, 229, 31, 147, 213, 172, 91, 250, 155, 122, 246, 13, 77, 131, 183, 181, 133, 216, 120, 192, 248, 202, 223, 231, 97, 149, 8, 237, 125, 183, 215, 12, 68, 97, 56, 60, 152, 89, 102, 110, 136, 169, 138, 147, 205, 84, 165, 128, 37, 92, 196, 160, 205, 106, 157, 173, 23, 236, 30, 216 } });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_RideId",
                table: "Payment",
                column: "RideId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payment_RideId",
                table: "Payment");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2024, 4, 24, 20, 6, 30, 188, DateTimeKind.Utc).AddTicks(3115), new byte[] { 55, 89, 82, 137, 81, 189, 131, 61, 136, 202, 220, 116, 127, 40, 34, 142, 242, 122, 51, 51, 184, 144, 160, 68, 14, 179, 79, 219, 159, 251, 83, 8, 56, 128, 219, 72, 201, 16, 53, 188, 144, 25, 50, 18, 36, 225, 244, 206, 110, 79, 236, 5, 252, 153, 174, 143, 180, 198, 33, 187, 208, 245, 223, 174 }, new byte[] { 162, 61, 15, 16, 243, 207, 42, 61, 64, 191, 185, 103, 117, 52, 221, 128, 106, 54, 57, 124, 148, 185, 124, 32, 180, 183, 180, 103, 180, 28, 141, 31, 98, 199, 179, 46, 246, 255, 175, 67, 98, 173, 57, 149, 130, 230, 230, 15, 79, 73, 152, 61, 120, 232, 99, 57, 219, 243, 22, 186, 11, 55, 79, 129, 142, 246, 59, 34, 177, 149, 128, 251, 31, 0, 238, 251, 232, 36, 147, 188, 13, 170, 117, 171, 150, 145, 58, 91, 60, 151, 92, 92, 25, 114, 174, 99, 23, 195, 104, 18, 8, 186, 240, 252, 201, 225, 52, 252, 216, 100, 26, 167, 210, 73, 254, 54, 217, 61, 141, 218, 252, 47, 72, 251, 98, 74, 169, 248 } });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_RideId",
                table: "Payment",
                column: "RideId");
        }
    }
}
