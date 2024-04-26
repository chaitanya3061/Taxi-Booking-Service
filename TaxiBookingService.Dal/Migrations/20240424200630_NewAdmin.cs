using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxiBookingService.Dal.Migrations
{
    /// <inheritdoc />
    public partial class NewAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admin",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CountryCode", "CreatedAt", "Email", "IsDeleted", "ModifiedAt", "Name", "PasswordHash", "PasswordSalt", "PhoneNumber", "RefreshToken", "RoleId", "TokenCreated", "TokenExpires" },
                values: new object[] { 101, "+91", new DateTime(2024, 4, 24, 20, 6, 30, 188, DateTimeKind.Utc).AddTicks(3115), "chaitu@gmail.com", false, null, "Chaitu", new byte[] { 55, 89, 82, 137, 81, 189, 131, 61, 136, 202, 220, 116, 127, 40, 34, 142, 242, 122, 51, 51, 184, 144, 160, 68, 14, 179, 79, 219, 159, 251, 83, 8, 56, 128, 219, 72, 201, 16, 53, 188, 144, 25, 50, 18, 36, 225, 244, 206, 110, 79, 236, 5, 252, 153, 174, 143, 180, 198, 33, 187, 208, 245, 223, 174 }, new byte[] { 162, 61, 15, 16, 243, 207, 42, 61, 64, 191, 185, 103, 117, 52, 221, 128, 106, 54, 57, 124, 148, 185, 124, 32, 180, 183, 180, 103, 180, 28, 141, 31, 98, 199, 179, 46, 246, 255, 175, 67, 98, 173, 57, 149, 130, 230, 230, 15, 79, 73, 152, 61, 120, 232, 99, 57, 219, 243, 22, 186, 11, 55, 79, 129, 142, 246, 59, 34, 177, 149, 128, 251, 31, 0, 238, 251, 232, 36, 147, 188, 13, 170, 117, 171, 150, 145, 58, 91, 60, 151, 92, 92, 25, 114, 174, 99, 23, 195, 104, 18, 8, 186, 240, 252, 201, 225, 52, 252, 216, 100, 26, 167, 210, 73, 254, 54, 217, 61, 141, 218, 252, 47, 72, 251, 98, 74, 169, 248 }, "7093605314", null, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Admin",
                columns: new[] { "Id", "UserId" },
                values: new object[] { 3, 101 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admin",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CountryCode", "CreatedAt", "Email", "IsDeleted", "ModifiedAt", "Name", "PasswordHash", "PasswordSalt", "PhoneNumber", "RefreshToken", "RoleId", "TokenCreated", "TokenExpires" },
                values: new object[] { 100, "+91", new DateTime(2024, 4, 24, 20, 3, 37, 320, DateTimeKind.Utc).AddTicks(8223), "chaitanya@gmail.com", false, null, "Chaitanya", new byte[] { 240, 205, 101, 75, 26, 120, 108, 216, 172, 235, 253, 188, 183, 40, 77, 62, 0, 111, 200, 246, 183, 219, 176, 48, 167, 239, 180, 105, 45, 32, 59, 183, 76, 108, 140, 185, 241, 81, 22, 23, 24, 245, 78, 152, 219, 208, 213, 117, 0, 164, 55, 99, 135, 68, 150, 39, 84, 147, 139, 158, 48, 225, 122, 128 }, new byte[] { 103, 116, 99, 150, 156, 99, 44, 142, 115, 150, 101, 103, 213, 120, 102, 96, 179, 142, 227, 72, 56, 142, 39, 205, 249, 39, 128, 220, 47, 249, 153, 221, 145, 112, 227, 240, 11, 10, 7, 44, 50, 129, 116, 16, 148, 223, 238, 227, 39, 195, 195, 206, 128, 129, 16, 184, 84, 140, 235, 8, 198, 64, 162, 201, 40, 14, 70, 120, 234, 1, 226, 138, 158, 172, 145, 154, 236, 70, 11, 5, 135, 103, 91, 52, 0, 154, 136, 79, 99, 86, 71, 186, 130, 22, 147, 219, 95, 192, 186, 22, 255, 43, 128, 174, 168, 149, 55, 154, 55, 79, 151, 97, 138, 69, 103, 123, 251, 89, 63, 119, 210, 164, 104, 62, 81, 206, 33, 250 }, "7093605314", null, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Admin",
                columns: new[] { "Id", "UserId" },
                values: new object[] { 1, 100 });
        }
    }
}
