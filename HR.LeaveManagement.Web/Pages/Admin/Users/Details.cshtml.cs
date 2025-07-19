using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Pages.Admin.Users
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public new ApplicationUser User { get; set; } = new ApplicationUser();
        public List<LeaveRequest> RecentLeaveRequests { get; set; } = new List<LeaveRequest>();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            User = user;

            // Load recent leave requests for this user
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == user.Email);

            if (employee != null)
            {
                RecentLeaveRequests = await _context.LeaveRequests
                    .Include(lr => lr.LeaveType)
                    .Where(lr => lr.EmployeeID == employee.EmployeeID)
                    .OrderByDescending(lr => lr.CreatedAt)
                    .Take(10)
                    .ToListAsync();
            }

            return Page();
        }
    }
}
