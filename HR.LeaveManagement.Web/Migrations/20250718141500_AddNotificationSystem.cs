using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HR.LeaveManagement.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationTemplates",
                columns: table => new
                {
                    TemplateID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Subject = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Body = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTemplates", x => x.TemplateID);
                });

            migrationBuilder.CreateTable(
                name: "NotificationLogs",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TemplateID = table.Column<int>(type: "INTEGER", nullable: false),
                    RecipientEmail = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    RecipientName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Subject = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Body = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SentAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ErrorMessage = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    RelatedEntityID = table.Column<int>(type: "INTEGER", nullable: true),
                    RelatedEntityType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationLogs", x => x.LogID);
                    table.ForeignKey(
                        name: "FK_NotificationLogs_NotificationTemplates_TemplateID",
                        column: x => x.TemplateID,
                        principalTable: "NotificationTemplates",
                        principalColumn: "TemplateID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "NotificationTemplates",
                columns: new[] { "TemplateID", "Body", "Category", "CreatedAt", "IsActive", "Name", "Subject", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "<h3>New Leave Request Submitted</h3>\n                            <p><strong>Employee:</strong> {EmployeeName}</p>\n                            <p><strong>Leave Type:</strong> {LeaveType}</p>\n                            <p><strong>From:</strong> {FromDate}</p>\n                            <p><strong>To:</strong> {ToDate}</p>\n                            <p><strong>Duration:</strong> {Duration} days</p>\n                            <p><strong>Reason:</strong> {Reason}</p>\n                            <p><strong>Request ID:</strong> {RequestId}</p>\n                            <p>Please review and approve/reject this leave request.</p>", "LeaveRequest", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "LeaveRequestSubmitted", "New Leave Request - {EmployeeName}", "Email", null },
                    { 2, "<h3>Leave Request Approved</h3>\n                            <p>Dear {EmployeeName},</p>\n                            <p>Your leave request has been approved!</p>\n                            <p><strong>Leave Type:</strong> {LeaveType}</p>\n                            <p><strong>From:</strong> {FromDate}</p>\n                            <p><strong>To:</strong> {ToDate}</p>\n                            <p><strong>Duration:</strong> {Duration} days</p>\n                            <p><strong>Comments:</strong> {Comments}</p>\n                            <p>Enjoy your time off!</p>", "LeaveRequest", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "LeaveRequestApproved", "Leave Request Approved - {LeaveType}", "Email", null },
                    { 3, "<h3>Leave Request Rejected</h3>\n                            <p>Dear {EmployeeName},</p>\n                            <p>Unfortunately, your leave request has been rejected.</p>\n                            <p><strong>Leave Type:</strong> {LeaveType}</p>\n                            <p><strong>From:</strong> {FromDate}</p>\n                            <p><strong>To:</strong> {ToDate}</p>\n                            <p><strong>Duration:</strong> {Duration} days</p>\n                            <p><strong>Reason for Rejection:</strong> {Comments}</p>\n                            <p>Please contact HR if you have any questions.</p>", "LeaveRequest", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "LeaveRequestRejected", "Leave Request Rejected - {LeaveType}", "Email", null },
                    { 4, "<h3>Pending Leave Request Reminder</h3>\n                            <p>This is a reminder that you have a pending leave request to review:</p>\n                            <p><strong>Employee:</strong> {EmployeeName}</p>\n                            <p><strong>Leave Type:</strong> {LeaveType}</p>\n                            <p><strong>From:</strong> {FromDate}</p>\n                            <p><strong>To:</strong> {ToDate}</p>\n                            <p><strong>Duration:</strong> {Duration} days</p>\n                            <p><strong>Submitted:</strong> {CurrentDate}</p>\n                            <p>Please review and take action on this request.</p>", "Reminder", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "LeaveRequestReminder", "Reminder: Pending Leave Request - {EmployeeName}", "Email", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationLogs_TemplateID",
                table: "NotificationLogs",
                column: "TemplateID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationLogs");

            migrationBuilder.DropTable(
                name: "NotificationTemplates");
        }
    }
}
