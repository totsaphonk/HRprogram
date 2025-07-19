using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Pages.Admin
{
    public class UsersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public UserStatistics UserStatistics { get; set; } = new UserStatistics();
        public RoleDistribution RoleDistribution { get; set; } = new RoleDistribution();
        public string SearchTerm { get; set; } = string.Empty;
        public string StatusFilter { get; set; } = "all";

        public async Task OnGetAsync(string? searchTerm, string? status)
        {
            SearchTerm = searchTerm ?? string.Empty;
            StatusFilter = status ?? "all";

            await LoadUsers();
            await LoadStatistics();
            await LoadRoleDistribution();
        }

        private async Task LoadUsers()
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(u => 
                    u.FullName.Contains(SearchTerm) || 
                    u.Email.Contains(SearchTerm) ||
                    u.UserName.Contains(SearchTerm));
            }

            if (StatusFilter != "all")
            {
                bool isActive = StatusFilter == "active";
                query = query.Where(u => u.IsActive == isActive);
            }

            Users = await query
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }

        private async Task LoadStatistics()
        {
            var today = DateTime.Today;
            var thisWeek = today.AddDays(-7);
            var thisMonth = today.AddDays(-30);

            UserStatistics.TotalUsers = await _context.Users.CountAsync();
            UserStatistics.ActiveUsers = await _context.Users.Where(u => u.IsActive).CountAsync();
            UserStatistics.AdminUsers = await _context.Users.Where(u => u.Role == "System Admin" || u.Role == "HR Admin").CountAsync();
            UserStatistics.NewUsersThisMonth = await _context.Users.Where(u => u.CreatedAt >= thisMonth).CountAsync();
            UserStatistics.LoggedInToday = await _context.Users.Where(u => u.LastLoginAt >= today).CountAsync();
            UserStatistics.LoggedInThisWeek = await _context.Users.Where(u => u.LastLoginAt >= thisWeek).CountAsync();
            UserStatistics.NeverLoggedIn = await _context.Users.Where(u => u.LastLoginAt == null).CountAsync();
        }

        private async Task LoadRoleDistribution()
        {
            RoleDistribution.SystemAdmin = await _context.Users.Where(u => u.Role == "System Admin").CountAsync();
            RoleDistribution.HRAdmin = await _context.Users.Where(u => u.Role == "HR Admin").CountAsync();
            RoleDistribution.LineManager = await _context.Users.Where(u => u.Role == "Line Manager").CountAsync();
            RoleDistribution.Employee = await _context.Users.Where(u => u.Role == "Employee").CountAsync();
        }

        public async Task<IActionResult> OnGetToggleStatusAsync(string id)
        {
            return await OnPostToggleStatusAsync(id);
        }

        public async Task<IActionResult> OnPostToggleStatusAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return new JsonResult(new { success = false, message = "User not found" });
            }

            user.IsActive = !user.IsActive;
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true, message = $"User {(user.IsActive ? "activated" : "deactivated")} successfully" });
        }

        public async Task<IActionResult> OnGetResetPasswordAsync(string id)
        {
            return await OnPostResetPasswordAsync(id);
        }

        public async Task<IActionResult> OnPostResetPasswordAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return new JsonResult(new { success = false, message = "User not found" });
            }

            // Generate a new password
            var newPassword = GeneratePassword();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                return new JsonResult(new { success = true, newPassword = newPassword });
            }
            else
            {
                return new JsonResult(new { success = false, message = "Failed to reset password" });
            }
        }

        private string GeneratePassword()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            return new string(Enumerable.Repeat(chars, 12)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

    public class UserStatistics
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int AdminUsers { get; set; }
        public int NewUsersThisMonth { get; set; }
        public int LoggedInToday { get; set; }
        public int LoggedInThisWeek { get; set; }
        public int NeverLoggedIn { get; set; }
    }

    public class RoleDistribution
    {
        public int SystemAdmin { get; set; }
        public int HRAdmin { get; set; }
        public int LineManager { get; set; }
        public int Employee { get; set; }
    }
}
