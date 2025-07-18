using HR.LeaveManagement.Web.Models;

namespace HR.LeaveManagement.Web.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body, string? toName = null);
        Task<bool> SendLeaveRequestNotificationAsync(LeaveRequest leaveRequest, string notificationType);
        Task<bool> SendLeaveStatusNotificationAsync(LeaveRequest leaveRequest, string status, string? comments = null);
        Task<bool> SendReminderNotificationAsync(LeaveRequest leaveRequest, string recipientEmail);
        Task<List<NotificationLog>> GetNotificationHistoryAsync(int? entityId = null, string? entityType = null);
    }
}
