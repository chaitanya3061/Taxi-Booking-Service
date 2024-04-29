using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxiBookingService.Dal.Migrations
{
    /// <inheritdoc />
    public partial class RideStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { new DateTime(2024, 4, 26, 13, 4, 28, 455, DateTimeKind.Utc).AddTicks(4263), new byte[] { 4, 179, 159, 125, 214, 208, 218, 152, 8, 202, 46, 157, 172, 123, 13, 67, 212, 240, 110, 193, 219, 83, 130, 141, 94, 18, 15, 166, 226, 119, 83, 174, 209, 127, 238, 201, 52, 77, 74, 90, 73, 85, 191, 11, 166, 196, 165, 93, 167, 250, 144, 105, 215, 198, 173, 225, 147, 38, 121, 94, 100, 103, 177, 226 }, new byte[] { 252, 63, 231, 166, 250, 34, 141, 80, 78, 172, 232, 171, 203, 248, 154, 160, 6, 133, 231, 186, 12, 198, 55, 24, 87, 126, 189, 156, 163, 236, 218, 140, 200, 147, 137, 121, 94, 237, 8, 22, 37, 223, 242, 11, 160, 102, 151, 213, 10, 142, 67, 68, 48, 232, 172, 104, 133, 229, 99, 109, 227, 76, 158, 152, 220, 42, 201, 25, 22, 56, 199, 190, 82, 164, 217, 25, 154, 52, 212, 161, 117, 135, 67, 231, 48, 156, 189, 235, 200, 12, 170, 253, 43, 49, 106, 224, 219, 110, 86, 187, 3, 105, 200, 225, 227, 159, 203, 215, 83, 177, 212, 162, 51, 127, 213, 50, 249, 51, 136, 103, 103, 1, 119, 109, 117, 55, 126, 167 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { new DateTime(2024, 4, 26, 5, 8, 43, 196, DateTimeKind.Utc).AddTicks(6906), new byte[] { 68, 34, 19, 126, 187, 83, 100, 227, 72, 199, 18, 120, 125, 172, 9, 229, 69, 181, 192, 33, 221, 130, 254, 200, 45, 0, 212, 169, 231, 97, 225, 223, 113, 35, 225, 227, 13, 137, 228, 73, 39, 109, 200, 192, 46, 34, 33, 224, 138, 162, 79, 42, 113, 51, 40, 215, 15, 156, 199, 178, 164, 237, 143, 5 }, new byte[] { 12, 216, 78, 231, 125, 208, 198, 5, 97, 108, 183, 68, 2, 227, 125, 68, 126, 56, 36, 233, 25, 199, 45, 118, 117, 204, 35, 87, 121, 111, 24, 192, 244, 221, 235, 163, 54, 177, 93, 137, 234, 95, 91, 167, 94, 87, 76, 233, 118, 184, 78, 86, 76, 249, 61, 6, 153, 197, 246, 158, 192, 214, 185, 206, 100, 195, 117, 170, 54, 84, 169, 28, 97, 107, 56, 94, 45, 36, 115, 104, 81, 3, 89, 20, 14, 156, 88, 248, 234, 45, 153, 114, 152, 38, 239, 136, 236, 82, 14, 224, 3, 80, 127, 51, 197, 153, 50, 32, 58, 160, 132, 82, 219, 108, 212, 189, 236, 183, 19, 173, 44, 27, 247, 87, 51, 35, 126, 160 } });
        }
    }
}
