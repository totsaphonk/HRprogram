using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using HR.LeaveManagement.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Pages
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LeaveBalanceService _leaveBalanceService;

        public DashboardModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, LeaveBalanceService leaveBalanceService)
        {
            _context = context;
            _userManager = userManager;
            _leaveBalanceService = leaveBalanceService;
        }

        public ApplicationUser? CurrentUser { get; set; }
        public DashboardStatistics Statistics { get; set; } = new DashboardStatistics();
        public List<RecentActivity> RecentActivities { get; set; } = new List<RecentActivity>();
        public List<UserLeaveBalance> UserLeaveBalances { get; set; } = new List<UserLeaveBalance>();
        public ChartData ChartData { get; set; } = new ChartData();

        public async Task<IActionResult> OnGetAsync()
        {
            // Get current user
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser == null)
            {
                return RedirectToPage("/Identity/Account/Login");
            }

            await LoadStatistics();
            LoadRecentActivities();
            await LoadUserLeaveBalances();
            await LoadChartData();

            return Page();
        }

        private async Task LoadStatistics()
        {
            var today = DateTime.Today;
            
            Statistics.TotalEmployees = await _context.Employees.CountAsync();
            Statistics.PendingRequests = await _context.LeaveRequests.Where(lr => lr.Status == "Pending").CountAsync();
            Statistics.ApprovedToday = await _context.LeaveRequests
                .Where(lr => lr.Status == "Approved" && lr.CreatedAt.Date == today)
                .CountAsync();
            Statistics.TotalNotifications = await _context.NotificationLogs
                .Where(nl => nl.CreatedAt.Date == today)
                .CountAsync();
        }

        private void LoadRecentActivities()
        {
            // In production, this would come from audit logs
            RecentActivities = new List<RecentActivity>
            {
                new RecentActivity { Timestamp = DateTime.Now.AddMinutes(-5), Description = "Leave request submitted", UserName = "John Doe", Status = "Success" },
                new RecentActivity { Timestamp = DateTime.Now.AddMinutes(-12), Description = "Leave request approved", UserName = "HR Admin", Status = "Success" },
                new RecentActivity { Timestamp = DateTime.Now.AddMinutes(-18), Description = "Email notification sent", UserName = "System", Status = "Success" },
                new RecentActivity { Timestamp = DateTime.Now.AddMinutes(-25), Description = "User login", UserName = CurrentUser?.FullName ?? "Unknown", Status = "Success" },
                new RecentActivity { Timestamp = DateTime.Now.AddMinutes(-30), Description = "Leave balance updated", UserName = "System", Status = "Success" },
                new RecentActivity { Timestamp = DateTime.Now.AddMinutes(-45), Description = "New employee added", UserName = "HR Admin", Status = "Success" },
                new RecentActivity { Timestamp = DateTime.Now.AddMinutes(-60), Description = "System backup completed", UserName = "System", Status = "Success" },
                new RecentActivity { Timestamp = DateTime.Now.AddMinutes(-75), Description = "Password reset", UserName = "Jane Smith", Status = "Warning" }
            };
        }

        private async Task LoadUserLeaveBalances()
        {
            try
            {
                // Find the employee record for the current user
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Email == CurrentUser.Email);

                if (employee != null)
                {
                    var balances = await _leaveBalanceService.GetLeaveBalancesForEmployeeAsync(employee.EmployeeID);
                    foreach (var balance in balances)
                    {
                        UserLeaveBalances.Add(new UserLeaveBalance
                        {
                            LeaveTypeName = balance.LeaveTypeName,
                            AvailableDays = balance.RemainingDays,
                            TotalDays = balance.EntitledDays,
                            UsedDays = balance.UsedDays
                        });
                    }
                }
            }
            catch (Exception)
            {
                // If there's an error, continue without leave balances
                UserLeaveBalances = new List<UserLeaveBalance>();
            }
        }

        private async Task LoadChartData()
        {
            var today = DateTime.Today;
            var fourWeeksAgo = today.AddDays(-28);

            // Group leave requests by week
            var requests = await _context.LeaveRequests
                .Where(lr => lr.CreatedAt >= fourWeeksAgo)
                .ToListAsync();

            ChartData.Week1 = requests.Where(r => r.CreatedAt >= fourWeeksAgo && r.CreatedAt < fourWeeksAgo.AddDays(7)).Count();
            ChartData.Week2 = requests.Where(r => r.CreatedAt >= fourWeeksAgo.AddDays(7) && r.CreatedAt < fourWeeksAgo.AddDays(14)).Count();
            ChartData.Week3 = requests.Where(r => r.CreatedAt >= fourWeeksAgo.AddDays(14) && r.CreatedAt < fourWeeksAgo.AddDays(21)).Count();
            ChartData.Week4 = requests.Where(r => r.CreatedAt >= fourWeeksAgo.AddDays(21)).Count();
        }
    }

    public class DashboardStatistics
    {
        public int TotalEmployees { get; set; }
        public int PendingRequests { get; set; }
        public int ApprovedToday { get; set; }
        public int TotalNotifications { get; set; }
    }

    public class RecentActivity
    {
        public DateTime Timestamp { get; set; }
        public string Description { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class UserLeaveBalance
    {
        public string LeaveTypeName { get; set; } = string.Empty;
        public int AvailableDays { get; set; }
        public int TotalDays { get; set; }
        public int UsedDays { get; set; }
    }

    public class ChartData
    {
        public int Week1 { get; set; }
        public int Week2 { get; set; }
        public int Week3 { get; set; }
        public int Week4 { get; set; }
    }
}
