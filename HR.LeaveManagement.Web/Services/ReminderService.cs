using HR.LeaveManagement.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Services
{
    public class ReminderService : IReminderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<ReminderService> _logger;

        public ReminderService(ApplicationDbContext context, IEmailService emailService, ILogger<ReminderService> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task SendPendingLeaveRequestRemindersAsync()
        {
            try
            {
                // Get leave requests that are pending for more than 2 days
                var pendingRequests = await _context.LeaveRequests
                    .Include(lr => lr.Employee)
                    .Include(lr => lr.LeaveType)
                    .Where(lr => lr.Status == "Pending" && lr.CreatedAt <= DateTime.UtcNow.AddDays(-2))
                    .ToListAsync();

                _logger.LogInformation("Found {Count} pending leave requests for reminders", pendingRequests.Count);

                var hrEmails = new List<string> { "hr@company.com", "manager@company.com" };

                foreach (var request in pendingRequests)
                {
                    foreach (var email in hrEmails)
                    {
                        try
                        {
                            await _emailService.SendReminderNotificationAsync(request, email);
                            _logger.LogInformation("Sent reminder for leave request {RequestId} to {Email}", request.RequestID, email);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to send reminder for leave request {RequestId} to {Email}", request.RequestID, email);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending pending leave request reminders");
            }
        }

        public async Task<int> GetPendingRequestCountAsync()
        {
            return await _context.LeaveRequests
                .Where(lr => lr.Status == "Pending")
                .CountAsync();
        }
    }
}
