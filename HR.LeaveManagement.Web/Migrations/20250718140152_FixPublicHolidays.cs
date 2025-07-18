using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HR.LeaveManagement.Web.Migrations
{
    /// <inheritdoc />
    public partial class FixPublicHolidays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PublicHolidays",
                columns: table => new
                {
                    HolidayID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    IsRecurring = table.Column<bool>(type: "INTEGER", nullable: false),
                    Region = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicHolidays", x => x.HolidayID);
                });

            migrationBuilder.InsertData(
                table: "PublicHolidays",
                columns: new[] { "HolidayID", "CreatedAt", "Date", "Description", "IsActive", "IsRecurring", "Name", "Region" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 18, 14, 1, 51, 304, DateTimeKind.Utc).AddTicks(9302), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "New Year celebration", true, true, "New Year's Day", "National" },
                    { 2, new DateTime(2025, 7, 18, 14, 1, 51, 305, DateTimeKind.Utc).AddTicks(1176), new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "International Workers' Day", true, true, "Labour Day", "National" },
                    { 3, new DateTime(2025, 7, 18, 14, 1, 51, 305, DateTimeKind.Utc).AddTicks(1179), new DateTime(2025, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Christmas celebration", true, true, "Christmas Day", "National" },
                    { 4, new DateTime(2025, 7, 18, 14, 1, 51, 305, DateTimeKind.Utc).AddTicks(1180), new DateTime(2025, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "National Independence Day", true, true, "National Day", "National" },
                    { 5, new DateTime(2025, 7, 18, 14, 1, 51, 305, DateTimeKind.Utc).AddTicks(1182), new DateTime(2025, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thanksgiving Day", true, true, "Thanksgiving", "National" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicHolidays");
        }
    }
}
