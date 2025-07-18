using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace HR.LeaveManagement.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(ApplicationDbContext context, IConfiguration configuration, ILogger<EmailService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body, string? toName = null)
        {
            try
            {
                var log = new NotificationLog
                {
                    RecipientEmail = to,
                    RecipientName = toName ?? to,
                    Subject = subject,
                    Body = body,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow,
                    TemplateID = 1 // Default template
                };

                _context.NotificationLogs.Add(log);
                await _context.SaveChangesAsync();

                // For development/demo purposes, we'll simulate sending email
                // In production, you would use actual SMTP configuration
                var smtpEnabled = _configuration.GetValue<bool>("EmailSettings:SmtpEnabled");
                
                if (smtpEnabled)
                {
                    await SendViaSmtp(to, subject, body);
                }
                else
                {
                    // Simulate email sending for demo
                    await Task.Delay(100);
                    _logger.LogInformation("Email sent to {Email}: {Subject}", to, subject);
                }

                // Update log status
                log.Status = "Sent";
                log.SentAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", to);
                
                // Update log with error
                var log = await _context.NotificationLogs
                    .Where(l => l.RecipientEmail == to && l.Status == "Pending")
                    .OrderByDescending(l => l.CreatedAt)
                    .FirstOrDefaultAsync();

                if (log != null)
                {
                    log.Status = "Failed";
                    log.ErrorMessage = ex.Message;
                    await _context.SaveChangesAsync();
                }

                return false;
            }
        }

        public async Task<bool> SendLeaveRequestNotificationAsync(LeaveRequest leaveRequest, string notificationType)
        {
            var employee = await _context.Employees.FindAsync(leaveRequest.EmployeeID);
            var leaveType = await _context.LeaveTypes.FindAsync(leaveRequest.LeaveTypeID);

            if (employee == null || leaveType == null)
            {
                _logger.LogWarning("Employee or LeaveType not found for LeaveRequest {RequestId}", leaveRequest.RequestID);
                return false;
            }

            var template = await GetNotificationTemplate(notificationType);
            if (template == null)
            {
                _logger.LogWarning("Notification template not found for type {Type}", notificationType);
                return false;
            }

            var subject = ReplaceTokens(template.Subject, leaveRequest, employee, leaveType);
            var body = ReplaceTokens(template.Body, leaveRequest, employee, leaveType);

            // Send to HR Admin (for new requests)
            if (notificationType == "LeaveRequestSubmitted")
            {
                var hrEmails = await GetHRAdminEmails();
                foreach (var email in hrEmails)
                {
                    await SendEmailAsync(email, subject, body);
                }
            }
            
            // Send to employee (for status updates)
            if (notificationType == "LeaveRequestApproved" || notificationType == "LeaveRequestRejected")
            {
                await SendEmailAsync(employee.Email, subject, body, employee.FullName);
            }

            return true;
        }

        public async Task<bool> SendLeaveStatusNotificationAsync(LeaveRequest leaveRequest, string status, string? comments = null)
        {
            var notificationType = status == "Approved" ? "LeaveRequestApproved" : "LeaveRequestRejected";
            return await SendLeaveRequestNotificationAsync(leaveRequest, notificationType);
        }

        public async Task<bool> SendReminderNotificationAsync(LeaveRequest leaveRequest, string recipientEmail)
        {
            var employee = await _context.Employees.FindAsync(leaveRequest.EmployeeID);
            var leaveType = await _context.LeaveTypes.FindAsync(leaveRequest.LeaveTypeID);

            if (employee == null || leaveType == null)
            {
                return false;
            }

            var template = await GetNotificationTemplate("LeaveRequestReminder");
            if (template == null)
            {
                return false;
            }

            var subject = ReplaceTokens(template.Subject, leaveRequest, employee, leaveType);
            var body = ReplaceTokens(template.Body, leaveRequest, employee, leaveType);

            return await SendEmailAsync(recipientEmail, subject, body);
        }

        public async Task<List<NotificationLog>> GetNotificationHistoryAsync(int? entityId = null, string? entityType = null)
        {
            var query = _context.NotificationLogs
                .Include(nl => nl.Template)
                .AsQueryable();

            if (entityId.HasValue && !string.IsNullOrEmpty(entityType))
            {
                query = query.Where(nl => nl.RelatedEntityID == entityId && nl.RelatedEntityType == entityType);
            }

            return await query
                .OrderByDescending(nl => nl.CreatedAt)
                .Take(100)
                .ToListAsync();
        }

        private async Task<NotificationTemplate?> GetNotificationTemplate(string templateName)
        {
            return await _context.NotificationTemplates
                .Where(t => t.Name == templateName && t.IsActive)
                .FirstOrDefaultAsync();
        }

        private string ReplaceTokens(string template, LeaveRequest leaveRequest, Employee employee, LeaveType leaveType)
        {
            return template
                .Replace("{EmployeeName}", employee.FullName)
                .Replace("{LeaveType}", leaveType.Name)
                .Replace("{FromDate}", leaveRequest.FromDate.ToString("MMM dd, yyyy"))
                .Replace("{ToDate}", leaveRequest.ToDate.ToString("MMM dd, yyyy"))
                .Replace("{Duration}", leaveRequest.Duration.ToString())
                .Replace("{Reason}", leaveRequest.Reason)
                .Replace("{Status}", leaveRequest.Status)
                .Replace("{RequestId}", leaveRequest.RequestID.ToString())
                .Replace("{Comments}", leaveRequest.Comments ?? "")
                .Replace("{CurrentDate}", DateTime.Now.ToString("MMM dd, yyyy"));
        }

        private Task<List<string>> GetHRAdminEmails()
        {
            // In a real application, you would query users with HR Admin role
            // For now, return a demo list
            return Task.FromResult(new List<string> { "hr@company.com", "manager@company.com" });
        }

        private async Task SendViaSmtp(string to, string subject, string body)
        {
            var smtpHost = _configuration["EmailSettings:SmtpHost"] ?? "smtp.gmail.com";
            var smtpPort = _configuration.GetValue<int>("EmailSettings:SmtpPort", 587);
            var smtpUsername = _configuration["EmailSettings:SmtpUsername"] ?? "";
            var smtpPassword = _configuration["EmailSettings:SmtpPassword"] ?? "";
            var fromEmail = _configuration["EmailSettings:FromEmail"] ?? "noreply@hrleave.com";

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail, "HR Leave Management System"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);

            await client.SendMailAsync(mailMessage);
        }
    }
}
