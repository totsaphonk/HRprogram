using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using HR.LeaveManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Pages.Admin
{
    public class SettingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public SettingsModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public SystemSettings Settings { get; set; } = new SystemSettings();

        public async Task OnGetAsync()
        {
            var settings = await _context.SystemSettings.FirstOrDefaultAsync();
            if (settings != null)
            {
                Settings = settings;
            }
        }

        public async Task<IActionResult> OnPostSaveGeneralAsync(SystemSettings settings)
        {
            await UpdateSettings(settings, "General");
            TempData["SuccessMessage"] = "General settings saved successfully.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSaveEmailAsync(SystemSettings settings)
        {
            await UpdateSettings(settings, "Email");
            TempData["SuccessMessage"] = "Email settings saved successfully.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSaveLeaveAsync(SystemSettings settings)
        {
            await UpdateSettings(settings, "Leave");
            TempData["SuccessMessage"] = "Leave settings saved successfully.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSaveNotificationAsync(SystemSettings settings)
        {
            await UpdateSettings(settings, "Notification");
            TempData["SuccessMessage"] = "Notification settings saved successfully.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSaveSecurityAsync(SystemSettings settings)
        {
            await UpdateSettings(settings, "Security");
            TempData["SuccessMessage"] = "Security settings saved successfully.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostTestEmailAsync()
        {
            try
            {
                var success = await _emailService.SendEmailAsync(
                    "test@example.com",
                    "Test Email - HR Leave Management System",
                    "<h3>Test Email</h3><p>This is a test email from the HR Leave Management System.</p><p>If you received this email, your email configuration is working correctly.</p>",
                    "Test User"
                );

                return new JsonResult(new { success = success, message = success ? "Test email sent successfully!" : "Failed to send test email" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        private async Task UpdateSettings(SystemSettings newSettings, string category)
        {
            var existingSettings = await _context.SystemSettings.FirstOrDefaultAsync();
            
            if (existingSettings == null)
            {
                existingSettings = new SystemSettings
                {
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.SystemSettings.Add(existingSettings);
            }
            else
            {
                existingSettings.UpdatedAt = DateTime.UtcNow;
            }

            // Update only the relevant properties based on category
            switch (category)
            {
                case "General":
                    existingSettings.CompanyName = newSettings.CompanyName;
                    existingSettings.SystemTitle = newSettings.SystemTitle;
                    existingSettings.TimeZone = newSettings.TimeZone;
                    existingSettings.DateFormat = newSettings.DateFormat;
                    existingSettings.LogoUrl = newSettings.LogoUrl;
                    existingSettings.MaintenanceMode = newSettings.MaintenanceMode;
                    break;

                case "Email":
                    existingSettings.SmtpHost = newSettings.SmtpHost;
                    existingSettings.SmtpPort = newSettings.SmtpPort;
                    existingSettings.SmtpUsername = newSettings.SmtpUsername;
                    existingSettings.SmtpPassword = newSettings.SmtpPassword;
                    existingSettings.FromEmail = newSettings.FromEmail;
                    existingSettings.FromName = newSettings.FromName;
                    existingSettings.EnableSsl = newSettings.EnableSsl;
                    existingSettings.EmailEnabled = newSettings.EmailEnabled;
                    break;

                case "Leave":
                    existingSettings.DefaultAnnualLeaveDays = newSettings.DefaultAnnualLeaveDays;
                    existingSettings.DefaultSickLeaveDays = newSettings.DefaultSickLeaveDays;
                    existingSettings.AdvanceNoticeRequired = newSettings.AdvanceNoticeRequired;
                    existingSettings.MaxConsecutiveLeaveDays = newSettings.MaxConsecutiveLeaveDays;
                    existingSettings.AllowWeekendLeave = newSettings.AllowWeekendLeave;
                    existingSettings.RequireManagerApproval = newSettings.RequireManagerApproval;
                    break;

                case "Notification":
                    existingSettings.NotifyOnLeaveSubmission = newSettings.NotifyOnLeaveSubmission;
                    existingSettings.NotifyOnLeaveApproval = newSettings.NotifyOnLeaveApproval;
                    existingSettings.SendReminders = newSettings.SendReminders;
                    existingSettings.ReminderFrequency = newSettings.ReminderFrequency;
                    existingSettings.ReminderAfterDays = newSettings.ReminderAfterDays;
                    break;

                case "Security":
                    existingSettings.SessionTimeout = newSettings.SessionTimeout;
                    existingSettings.PasswordMinLength = newSettings.PasswordMinLength;
                    existingSettings.RequirePasswordComplexity = newSettings.RequirePasswordComplexity;
                    existingSettings.EnableAuditLog = newSettings.EnableAuditLog;
                    existingSettings.EnableTwoFactorAuth = newSettings.EnableTwoFactorAuth;
                    break;
            }

            await _context.SaveChangesAsync();
        }
    }
}
