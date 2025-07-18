namespace HR.LeaveManagement.Web.Services
{
    public interface IReminderService
    {
        Task SendPendingLeaveRequestRemindersAsync();
        Task<int> GetPendingRequestCountAsync();
    }
}
