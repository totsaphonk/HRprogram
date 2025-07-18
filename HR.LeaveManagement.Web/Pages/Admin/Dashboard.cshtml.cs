using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public SystemStatistics Statistics { get; set; } = new SystemStatistics();
        public SystemHealth SystemHealth { get; set; } = new SystemHealth();
        public List<RecentActivity> RecentActivities { get; set; } = new List<RecentActivity>();
        public TodaysSummary TodaysSummary { get; set; } = new TodaysSummary();

        public async Task OnGetAsync()
        {
            await LoadStatistics();
            LoadSystemHealth();
            LoadRecentActivities();
            await LoadTodaysSummary();
        }

        private async Task LoadStatistics()
        {
            Statistics.TotalEmployees = await _context.Employees.CountAsync();
            Statistics.TotalLeaveRequests = await _context.LeaveRequests.CountAsync();
            Statistics.PendingRequests = await _context.LeaveRequests.Where(lr => lr.Status == "Pending").CountAsync();
            Statistics.TotalNotifications = await _context.NotificationLogs.CountAsync();
            Statistics.TotalUsers = await _context.Users.CountAsync();
        }

        private void LoadSystemHealth()
        {
            SystemHealth.EmailServiceStatus = "Online"; // In production, check actual service status
            SystemHealth.SystemLoad = 45; // In production, get actual system metrics
            SystemHealth.DiskUsage = 67; // In production, get actual disk usage
            SystemHealth.Uptime = GetUptime();
        }

        private void LoadRecentActivities()
        {
            // In production, this would come from audit logs
            RecentActivities = new List<RecentActivity>
            {
                new RecentActivity { Timestamp = DateTime.Now.AddMinutes(-5), Description = "Leave request approved", UserName = "HR Admin", Status = "Success" },
                new RecentActivity { Timestamp = DateTime.Now.AddMinutes(-12), Description = "New employee registered", UserName = "System", Status = "Success" },
                new RecentActivity { Timestamp = DateTime.Now.AddMinutes(-18), Description = "Email notification sent", UserName = "System", Status = "Success" },
                new RecentActivity { Timestamp = DateTime.Now.AddMinutes(-25), Description = "Leave request submitted", UserName = "John Doe", Status = "Success" },
                new RecentActivity { Timestamp = DateTime.Now.AddMinutes(-30), Description = "Failed to send email", UserName = "System", Status = "Error" }
            };
        }

        private async Task LoadTodaysSummary()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            TodaysSummary.NewLeaveRequests = await _context.LeaveRequests
                .Where(lr => lr.CreatedAt >= today && lr.CreatedAt < tomorrow)
                .CountAsync();

            TodaysSummary.ApprovedRequests = await _context.LeaveRequests
                .Where(lr => lr.Status == "Approved" && lr.CreatedAt >= today && lr.CreatedAt < tomorrow)
                .CountAsync();

            TodaysSummary.RejectedRequests = await _context.LeaveRequests
                .Where(lr => lr.Status == "Rejected" && lr.CreatedAt >= today && lr.CreatedAt < tomorrow)
                .CountAsync();

            TodaysSummary.NotificationsSent = await _context.NotificationLogs
                .Where(nl => nl.CreatedAt >= today && nl.CreatedAt < tomorrow && nl.Status == "Sent")
                .CountAsync();

            TodaysSummary.NewRegistrations = await _context.Users
                .Where(u => u.CreatedAt >= today && u.CreatedAt < tomorrow)
                .CountAsync();
        }

        private string GetUptime()
        {
            // In production, calculate actual uptime
            var uptime = TimeSpan.FromDays(7).Add(TimeSpan.FromHours(12)).Add(TimeSpan.FromMinutes(35));
            return $"{uptime.Days}d {uptime.Hours}h {uptime.Minutes}m";
        }
    }

    public class SystemStatistics
    {
        public int TotalEmployees { get; set; }
        public int TotalLeaveRequests { get; set; }
        public int PendingRequests { get; set; }
        public int TotalNotifications { get; set; }
        public int TotalUsers { get; set; }
    }

    public class SystemHealth
    {
        public string EmailServiceStatus { get; set; } = "Online";
        public int SystemLoad { get; set; }
        public int DiskUsage { get; set; }
        public string Uptime { get; set; } = "0d 0h 0m";
    }

    public class RecentActivity
    {
        public DateTime Timestamp { get; set; }
        public string Description { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class TodaysSummary
    {
        public int NewLeaveRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int RejectedRequests { get; set; }
        public int NotificationsSent { get; set; }
        public int NewRegistrations { get; set; }
    }
}
