using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using HR.LeaveManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Pages.Notifications
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public IndexModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public List<NotificationLog> NotificationLogs { get; set; } = new List<NotificationLog>();
        public string SearchTerm { get; set; } = string.Empty;
        public string StatusFilter { get; set; } = "all";

        public async Task OnGetAsync(string? searchTerm, string? status)
        {
            SearchTerm = searchTerm ?? string.Empty;
            StatusFilter = status ?? "all";

            var query = _context.NotificationLogs
                .Include(nl => nl.Template)
                .AsQueryable();

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(nl => 
                    nl.RecipientEmail.Contains(SearchTerm) || 
                    nl.RecipientName.Contains(SearchTerm) ||
                    nl.Subject.Contains(SearchTerm));
            }

            if (StatusFilter != "all")
            {
                query = query.Where(nl => nl.Status.ToLower() == StatusFilter.ToLower());
            }

            NotificationLogs = await query
                .OrderByDescending(nl => nl.CreatedAt)
                .Take(50)
                .ToListAsync();
        }

        public async Task<IActionResult> OnGetDetailsAsync(int id)
        {
            var log = await _context.NotificationLogs
                .Include(nl => nl.Template)
                .FirstOrDefaultAsync(nl => nl.LogID == id);

            if (log == null)
            {
                return NotFound();
            }

            var result = new
            {
                recipientName = log.RecipientName,
                recipientEmail = log.RecipientEmail,
                templateName = log.Template.Name,
                status = log.Status,
                createdAt = log.CreatedAt,
                sentAt = log.SentAt,
                subject = log.Subject,
                body = log.Body,
                errorMessage = log.ErrorMessage
            };

            return new JsonResult(result);
        }

        public async Task<IActionResult> OnPostRetryAsync(int id)
        {
            var log = await _context.NotificationLogs.FindAsync(id);
            
            if (log == null)
            {
                return new JsonResult(new { success = false, message = "Notification not found" });
            }

            if (log.Status != "Failed")
            {
                return new JsonResult(new { success = false, message = "Only failed notifications can be retried" });
            }

            try
            {
                var success = await _emailService.SendEmailAsync(log.RecipientEmail, log.Subject, log.Body, log.RecipientName);
                
                if (success)
                {
                    log.Status = "Sent";
                    log.SentAt = DateTime.UtcNow;
                    log.ErrorMessage = string.Empty;
                    await _context.SaveChangesAsync();
                }

                return new JsonResult(new { success = success, message = success ? "Notification sent successfully" : "Failed to send notification" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }
    }
}
