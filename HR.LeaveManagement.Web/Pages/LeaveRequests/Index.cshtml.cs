using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Pages.LeaveRequests
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<LeaveRequest> LeaveRequests { get; set; } = default!;
        public IList<Employee> Employees { get; set; } = default!;
        public IList<LeaveType> LeaveTypes { get; set; } = default!;
        
        public string? StatusFilter { get; set; }
        public int? EmployeeFilter { get; set; }
        public int? LeaveTypeFilter { get; set; }

        public async Task OnGetAsync(string? status, int? employeeId, int? leaveTypeId)
        {
            StatusFilter = status;
            EmployeeFilter = employeeId;
            LeaveTypeFilter = leaveTypeId;

            var query = _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.LeaveType)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(lr => lr.Status == status);
            }

            if (employeeId.HasValue)
            {
                query = query.Where(lr => lr.EmployeeID == employeeId.Value);
            }

            if (leaveTypeId.HasValue)
            {
                query = query.Where(lr => lr.LeaveTypeID == leaveTypeId.Value);
            }

            LeaveRequests = await query
                .OrderByDescending(lr => lr.CreatedAt)
                .ToListAsync();

            // Load filter options
            Employees = await _context.Employees
                .Where(e => e.ActiveStatus)
                .OrderBy(e => e.FullName)
                .ToListAsync();

            LeaveTypes = await _context.LeaveTypes
                .OrderBy(lt => lt.Name)
                .ToListAsync();
        }
    }
}
