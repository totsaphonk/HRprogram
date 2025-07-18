using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.LeaveManagement.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    SettingsID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SystemTitle = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    TimeZone = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DateFormat = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    LogoUrl = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    MaintenanceMode = table.Column<bool>(type: "INTEGER", nullable: false),
                    SmtpHost = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SmtpPort = table.Column<int>(type: "INTEGER", nullable: false),
                    SmtpUsername = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SmtpPassword = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    FromEmail = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    FromName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    EnableSsl = table.Column<bool>(type: "INTEGER", nullable: false),
                    EmailEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    DefaultAnnualLeaveDays = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultSickLeaveDays = table.Column<int>(type: "INTEGER", nullable: false),
                    AdvanceNoticeRequired = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxConsecutiveLeaveDays = table.Column<int>(type: "INTEGER", nullable: false),
                    AllowWeekendLeave = table.Column<bool>(type: "INTEGER", nullable: false),
                    RequireManagerApproval = table.Column<bool>(type: "INTEGER", nullable: false),
                    NotifyOnLeaveSubmission = table.Column<bool>(type: "INTEGER", nullable: false),
                    NotifyOnLeaveApproval = table.Column<bool>(type: "INTEGER", nullable: false),
                    SendReminders = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReminderFrequency = table.Column<int>(type: "INTEGER", nullable: false),
                    ReminderAfterDays = table.Column<int>(type: "INTEGER", nullable: false),
                    SessionTimeout = table.Column<int>(type: "INTEGER", nullable: false),
                    PasswordMinLength = table.Column<int>(type: "INTEGER", nullable: false),
                    RequirePasswordComplexity = table.Column<bool>(type: "INTEGER", nullable: false),
                    EnableAuditLog = table.Column<bool>(type: "INTEGER", nullable: false),
                    EnableTwoFactorAuth = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.SettingsID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemSettings");
        }
    }
}
