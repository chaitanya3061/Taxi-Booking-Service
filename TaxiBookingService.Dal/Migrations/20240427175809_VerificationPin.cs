using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxiBookingService.Dal.Migrations
{
    /// <inheritdoc />
    public partial class VerificationPin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VerificationPin",
                table: "Ride",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2024, 4, 27, 17, 58, 9, 69, DateTimeKind.Utc).AddTicks(7571), new byte[] { 101, 37, 162, 4, 55, 114, 135, 94, 198, 5, 42, 28, 244, 239, 43, 207, 90, 149, 52, 107, 75, 134, 67, 50, 100, 55, 156, 90, 160, 73, 77, 103, 153, 62, 142, 116, 22, 155, 98, 42, 223, 110, 222, 149, 118, 133, 152, 132, 58, 87, 145, 195, 117, 208, 218, 230, 130, 55, 209, 37, 143, 11, 175, 90 }, new byte[] { 32, 231, 32, 141, 4, 119, 77, 197, 204, 56, 146, 192, 159, 85, 234, 4, 255, 134, 121, 31, 91, 205, 203, 106, 197, 196, 232, 63, 184, 5, 67, 68, 31, 142, 229, 187, 238, 198, 210, 226, 28, 169, 118, 244, 252, 123, 151, 151, 13, 193, 27, 169, 109, 24, 200, 44, 88, 203, 114, 246, 159, 156, 57, 214, 213, 184, 29, 36, 227, 64, 62, 153, 84, 47, 202, 19, 125, 13, 4, 208, 80, 126, 100, 249, 47, 133, 222, 30, 20, 55, 59, 185, 224, 223, 53, 163, 80, 219, 172, 26, 95, 57, 165, 228, 167, 250, 200, 153, 198, 17, 49, 32, 87, 163, 134, 159, 10, 211, 163, 215, 212, 37, 250, 90, 193, 10, 62, 230 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationPin",
                table: "Ride");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2024, 4, 27, 11, 22, 17, 544, DateTimeKind.Utc).AddTicks(6695), new byte[] { 206, 41, 50, 173, 127, 182, 32, 159, 160, 4, 84, 122, 53, 126, 55, 177, 180, 116, 136, 150, 177, 178, 81, 101, 159, 205, 69, 129, 191, 67, 191, 197, 9, 59, 134, 196, 65, 110, 156, 80, 158, 195, 171, 175, 136, 119, 172, 196, 203, 72, 180, 63, 52, 25, 14, 175, 199, 86, 136, 218, 143, 159, 65, 90 }, new byte[] { 190, 40, 69, 162, 164, 160, 170, 78, 232, 11, 13, 192, 212, 91, 26, 52, 150, 52, 47, 75, 114, 216, 13, 55, 185, 135, 68, 167, 23, 39, 113, 146, 32, 229, 122, 242, 39, 77, 87, 183, 141, 166, 34, 233, 249, 250, 88, 174, 169, 46, 9, 78, 53, 149, 42, 126, 151, 116, 1, 106, 57, 16, 103, 53, 145, 231, 195, 211, 235, 14, 82, 243, 203, 39, 15, 87, 1, 6, 244, 185, 44, 88, 130, 169, 37, 228, 70, 22, 147, 32, 73, 149, 189, 3, 120, 32, 241, 45, 97, 123, 178, 114, 184, 158, 6, 90, 245, 37, 35, 71, 96, 204, 69, 225, 94, 129, 125, 236, 162, 171, 182, 10, 164, 244, 190, 84, 87, 192 } });
        }
    }
}
