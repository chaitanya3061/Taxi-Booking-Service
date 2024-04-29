using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxiBookingService.Dal.Migrations
{
    /// <inheritdoc />
    public partial class CustomerDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PenaltyFee",
                table: "Customer",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2024, 4, 26, 5, 8, 43, 196, DateTimeKind.Utc).AddTicks(6906), new byte[] { 68, 34, 19, 126, 187, 83, 100, 227, 72, 199, 18, 120, 125, 172, 9, 229, 69, 181, 192, 33, 221, 130, 254, 200, 45, 0, 212, 169, 231, 97, 225, 223, 113, 35, 225, 227, 13, 137, 228, 73, 39, 109, 200, 192, 46, 34, 33, 224, 138, 162, 79, 42, 113, 51, 40, 215, 15, 156, 199, 178, 164, 237, 143, 5 }, new byte[] { 12, 216, 78, 231, 125, 208, 198, 5, 97, 108, 183, 68, 2, 227, 125, 68, 126, 56, 36, 233, 25, 199, 45, 118, 117, 204, 35, 87, 121, 111, 24, 192, 244, 221, 235, 163, 54, 177, 93, 137, 234, 95, 91, 167, 94, 87, 76, 233, 118, 184, 78, 86, 76, 249, 61, 6, 153, 197, 246, 158, 192, 214, 185, 206, 100, 195, 117, 170, 54, 84, 169, 28, 97, 107, 56, 94, 45, 36, 115, 104, 81, 3, 89, 20, 14, 156, 88, 248, 234, 45, 153, 114, 152, 38, 239, 136, 236, 82, 14, 224, 3, 80, 127, 51, 197, 153, 50, 32, 58, 160, 132, 82, 219, 108, 212, 189, 236, 183, 19, 173, 44, 27, 247, 87, 51, 35, 126, 160 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PenaltyFee",
                table: "Customer");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2024, 4, 26, 4, 59, 7, 66, DateTimeKind.Utc).AddTicks(7613), new byte[] { 226, 116, 200, 191, 108, 37, 184, 0, 122, 26, 58, 81, 224, 77, 108, 19, 232, 89, 100, 252, 89, 117, 55, 59, 23, 167, 149, 250, 230, 128, 98, 52, 227, 116, 149, 81, 62, 135, 203, 62, 189, 162, 15, 223, 23, 104, 125, 105, 39, 223, 252, 254, 29, 197, 28, 53, 8, 239, 221, 204, 27, 249, 65, 10 }, new byte[] { 155, 222, 18, 235, 104, 189, 176, 143, 224, 243, 168, 169, 96, 166, 225, 216, 19, 124, 39, 18, 74, 101, 104, 126, 39, 79, 196, 137, 41, 1, 255, 150, 11, 100, 73, 12, 118, 69, 57, 53, 220, 212, 100, 97, 159, 29, 140, 237, 134, 61, 50, 5, 62, 100, 90, 233, 95, 181, 102, 175, 68, 22, 22, 51, 237, 234, 204, 176, 113, 47, 184, 12, 138, 71, 45, 95, 237, 189, 76, 92, 192, 201, 167, 146, 61, 36, 55, 43, 161, 66, 187, 74, 129, 249, 193, 148, 92, 109, 79, 157, 150, 128, 98, 153, 104, 233, 9, 80, 166, 201, 254, 62, 6, 102, 198, 198, 61, 32, 107, 156, 25, 3, 107, 105, 92, 132, 53, 234 } });
        }
    }
}
