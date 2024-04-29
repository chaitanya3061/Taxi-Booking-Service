using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxiBookingService.Dal.Migrations
{
    /// <inheritdoc />
    public partial class Cancellation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "RideStatus",
                keyColumn: "Id",
                keyValue: 5,
                column: "Status",
                value: "CustomerCancelled");

            migrationBuilder.InsertData(
                table: "RideStatus",
                columns: new[] { "Id", "Status" },
                values: new object[] { 6, "DriverCancelled" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2024, 4, 26, 4, 59, 7, 66, DateTimeKind.Utc).AddTicks(7613), new byte[] { 226, 116, 200, 191, 108, 37, 184, 0, 122, 26, 58, 81, 224, 77, 108, 19, 232, 89, 100, 252, 89, 117, 55, 59, 23, 167, 149, 250, 230, 128, 98, 52, 227, 116, 149, 81, 62, 135, 203, 62, 189, 162, 15, 223, 23, 104, 125, 105, 39, 223, 252, 254, 29, 197, 28, 53, 8, 239, 221, 204, 27, 249, 65, 10 }, new byte[] { 155, 222, 18, 235, 104, 189, 176, 143, 224, 243, 168, 169, 96, 166, 225, 216, 19, 124, 39, 18, 74, 101, 104, 126, 39, 79, 196, 137, 41, 1, 255, 150, 11, 100, 73, 12, 118, 69, 57, 53, 220, 212, 100, 97, 159, 29, 140, 237, 134, 61, 50, 5, 62, 100, 90, 233, 95, 181, 102, 175, 68, 22, 22, 51, 237, 234, 204, 176, 113, 47, 184, 12, 138, 71, 45, 95, 237, 189, 76, 92, 192, 201, 167, 146, 61, 36, 55, 43, 161, 66, 187, 74, 129, 249, 193, 148, 92, 109, 79, 157, 150, 128, 98, 153, 104, 233, 9, 80, 166, 201, 254, 62, 6, 102, 198, 198, 61, 32, 107, 156, 25, 3, 107, 105, 92, 132, 53, 234 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RideStatus",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "RideStatus",
                keyColumn: "Id",
                keyValue: 5,
                column: "Status",
                value: "Cancelled");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2024, 4, 25, 18, 12, 52, 370, DateTimeKind.Utc).AddTicks(7352), new byte[] { 155, 16, 185, 148, 156, 41, 120, 121, 80, 204, 30, 233, 158, 89, 94, 68, 108, 111, 218, 155, 69, 119, 185, 17, 44, 149, 177, 234, 173, 14, 101, 44, 182, 85, 107, 168, 115, 28, 139, 82, 228, 251, 96, 25, 15, 137, 189, 84, 121, 165, 166, 201, 143, 251, 2, 86, 224, 239, 11, 46, 151, 229, 239, 215 }, new byte[] { 223, 179, 25, 248, 104, 137, 197, 169, 82, 199, 144, 6, 139, 166, 47, 160, 181, 53, 207, 170, 134, 153, 192, 13, 27, 138, 241, 103, 218, 208, 58, 25, 40, 221, 26, 5, 89, 149, 135, 218, 107, 102, 238, 133, 125, 176, 25, 202, 113, 164, 233, 48, 217, 173, 68, 83, 133, 58, 176, 86, 214, 93, 29, 193, 138, 148, 200, 19, 101, 229, 31, 147, 213, 172, 91, 250, 155, 122, 246, 13, 77, 131, 183, 181, 133, 216, 120, 192, 248, 202, 223, 231, 97, 149, 8, 237, 125, 183, 215, 12, 68, 97, 56, 60, 152, 89, 102, 110, 136, 169, 138, 147, 205, 84, 165, 128, 37, 92, 196, 160, 205, 106, 157, 173, 23, 236, 30, 216 } });
        }
    }
}
