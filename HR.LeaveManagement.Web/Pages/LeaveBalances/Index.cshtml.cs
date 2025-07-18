using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using HR.LeaveManagement.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Pages.LeaveBalances
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly LeaveBalanceService _leaveBalanceService;

        public IndexModel(ApplicationDbContext context, LeaveBalanceService leaveBalanceService)
        {
            _context = context;
            _leaveBalanceService = leaveBalanceService;
        }

        public IList<Employee> Employees { get; set; } = default!;
        public IList<LeaveBalance> LeaveBalances { get; set; } = default!;
        public int? SelectedEmployeeId { get; set; }
        public int CurrentYear { get; set; } = DateTime.Now.Year;

        public async Task OnGetAsync(int? employeeId, int? year)
        {
            SelectedEmployeeId = employeeId;
            CurrentYear = year ?? DateTime.Now.Year;

            // Load all employees for selection
            Employees = await _context.Employees
                .Where(e => e.ActiveStatus)
                .OrderBy(e => e.FullName)
                .ToListAsync();

            // Load leave balances if employee is selected
            if (employeeId.HasValue)
            {
                LeaveBalances = await _leaveBalanceService.GetLeaveBalancesForEmployeeAsync(employeeId.Value);
            }
            else
            {
                LeaveBalances = new List<LeaveBalance>();
            }
        }
    }
}
