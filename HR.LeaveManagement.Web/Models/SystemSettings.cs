using System.ComponentModel.DataAnnotations;

namespace HR.LeaveManagement.Web.Models
{
    public class SystemSettings
    {
        [Key]
        public int SettingsID { get; set; }

        // General Settings
        [StringLength(100)]
        public string CompanyName { get; set; } = "HR Leave Management";

        [StringLength(100)]
        public string SystemTitle { get; set; } = "HR Leave Management System";

        [StringLength(50)]
        public string TimeZone { get; set; } = "UTC";

        [StringLength(20)]
        public string DateFormat { get; set; } = "MM/dd/yyyy";

        [StringLength(200)]
        public string LogoUrl { get; set; } = string.Empty;

        public bool MaintenanceMode { get; set; } = false;

        // Email Settings
        [StringLength(100)]
        public string SmtpHost { get; set; } = "smtp.gmail.com";

        public int SmtpPort { get; set; } = 587;

        [StringLength(100)]
        public string SmtpUsername { get; set; } = string.Empty;

        [StringLength(100)]
        public string SmtpPassword { get; set; } = string.Empty;

        [StringLength(100)]
        public string FromEmail { get; set; } = "noreply@company.com";

        [StringLength(100)]
        public string FromName { get; set; } = "HR Leave Management";

        public bool EnableSsl { get; set; } = true;

        public bool EmailEnabled { get; set; } = true;

        // Leave Settings
        public int DefaultAnnualLeaveDays { get; set; } = 21;

        public int DefaultSickLeaveDays { get; set; } = 10;

        public int AdvanceNoticeRequired { get; set; } = 2;

        public int MaxConsecutiveLeaveDays { get; set; } = 14;

        public bool AllowWeekendLeave { get; set; } = false;

        public bool RequireManagerApproval { get; set; } = true;

        // Notification Settings
        public bool NotifyOnLeaveSubmission { get; set; } = true;

        public bool NotifyOnLeaveApproval { get; set; } = true;

        public bool SendReminders { get; set; } = true;

        public int ReminderFrequency { get; set; } = 24;

        public int ReminderAfterDays { get; set; } = 2;

        // Security Settings
        public int SessionTimeout { get; set; } = 30;

        public int PasswordMinLength { get; set; } = 6;

        public bool RequirePasswordComplexity { get; set; } = true;

        public bool EnableAuditLog { get; set; } = true;

        public bool EnableTwoFactorAuth { get; set; } = false;

        // System
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
