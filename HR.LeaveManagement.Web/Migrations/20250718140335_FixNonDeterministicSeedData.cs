using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.LeaveManagement.Web.Migrations
{
    /// <inheritdoc />
    public partial class FixNonDeterministicSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PublicHolidays",
                keyColumn: "HolidayID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "PublicHolidays",
                keyColumn: "HolidayID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "PublicHolidays",
                keyColumn: "HolidayID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "PublicHolidays",
                keyColumn: "HolidayID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "PublicHolidays",
                keyColumn: "HolidayID",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PublicHolidays",
                keyColumn: "HolidayID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 18, 14, 1, 51, 304, DateTimeKind.Utc).AddTicks(9302));

            migrationBuilder.UpdateData(
                table: "PublicHolidays",
                keyColumn: "HolidayID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 18, 14, 1, 51, 305, DateTimeKind.Utc).AddTicks(1176));

            migrationBuilder.UpdateData(
                table: "PublicHolidays",
                keyColumn: "HolidayID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 18, 14, 1, 51, 305, DateTimeKind.Utc).AddTicks(1179));

            migrationBuilder.UpdateData(
                table: "PublicHolidays",
                keyColumn: "HolidayID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 18, 14, 1, 51, 305, DateTimeKind.Utc).AddTicks(1180));

            migrationBuilder.UpdateData(
                table: "PublicHolidays",
                keyColumn: "HolidayID",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 18, 14, 1, 51, 305, DateTimeKind.Utc).AddTicks(1182));
        }
    }
}
