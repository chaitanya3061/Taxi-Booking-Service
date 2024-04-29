using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxiBookingService.Dal.Migrations
{
    /// <inheritdoc />
    public partial class IsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Driver");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedAt", "IsActive", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2024, 4, 27, 8, 35, 4, 360, DateTimeKind.Utc).AddTicks(2289), false, new byte[] { 176, 137, 191, 116, 208, 178, 9, 156, 175, 233, 156, 145, 238, 170, 11, 126, 20, 114, 82, 255, 84, 86, 215, 112, 54, 31, 115, 96, 7, 126, 85, 160, 113, 131, 115, 172, 37, 25, 71, 7, 18, 16, 186, 56, 94, 31, 46, 237, 226, 13, 76, 74, 188, 151, 50, 167, 188, 178, 55, 44, 110, 202, 100, 202 }, new byte[] { 6, 240, 168, 151, 75, 154, 162, 3, 39, 154, 72, 136, 229, 61, 132, 48, 122, 10, 212, 104, 187, 226, 52, 106, 111, 129, 137, 107, 144, 158, 16, 118, 12, 113, 15, 167, 17, 182, 241, 151, 210, 200, 62, 1, 228, 246, 176, 60, 11, 209, 131, 142, 220, 77, 34, 203, 153, 229, 144, 156, 239, 191, 113, 62, 202, 13, 121, 214, 223, 117, 70, 43, 22, 153, 36, 131, 97, 138, 69, 251, 178, 6, 110, 27, 110, 122, 82, 84, 63, 34, 60, 107, 249, 46, 135, 104, 79, 191, 160, 139, 128, 240, 90, 81, 182, 178, 85, 110, 73, 9, 126, 143, 50, 212, 190, 44, 156, 48, 210, 47, 17, 108, 15, 124, 33, 205, 209, 24 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "User");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Driver",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2024, 4, 26, 13, 4, 28, 455, DateTimeKind.Utc).AddTicks(4263), new byte[] { 4, 179, 159, 125, 214, 208, 218, 152, 8, 202, 46, 157, 172, 123, 13, 67, 212, 240, 110, 193, 219, 83, 130, 141, 94, 18, 15, 166, 226, 119, 83, 174, 209, 127, 238, 201, 52, 77, 74, 90, 73, 85, 191, 11, 166, 196, 165, 93, 167, 250, 144, 105, 215, 198, 173, 225, 147, 38, 121, 94, 100, 103, 177, 226 }, new byte[] { 252, 63, 231, 166, 250, 34, 141, 80, 78, 172, 232, 171, 203, 248, 154, 160, 6, 133, 231, 186, 12, 198, 55, 24, 87, 126, 189, 156, 163, 236, 218, 140, 200, 147, 137, 121, 94, 237, 8, 22, 37, 223, 242, 11, 160, 102, 151, 213, 10, 142, 67, 68, 48, 232, 172, 104, 133, 229, 99, 109, 227, 76, 158, 152, 220, 42, 201, 25, 22, 56, 199, 190, 82, 164, 217, 25, 154, 52, 212, 161, 117, 135, 67, 231, 48, 156, 189, 235, 200, 12, 170, 253, 43, 49, 106, 224, 219, 110, 86, 187, 3, 105, 200, 225, 227, 159, 203, 215, 83, 177, 212, 162, 51, 127, 213, 50, 249, 51, 136, 103, 103, 1, 119, 109, 117, 55, 126, 167 } });
        }
    }
}
