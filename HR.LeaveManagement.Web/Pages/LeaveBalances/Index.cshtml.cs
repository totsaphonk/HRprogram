using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using HR.LeaveManagement.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace HR.LeaveManagement.Web.Pages.LeaveBalances
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly LeaveBalanceService _leaveBalanceService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ApplicationDbContext context, LeaveBalanceService leaveBalanceService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _leaveBalanceService = leaveBalanceService;
            _userManager = userManager;
        }

        public IList<Employee> Employees { get; set; } = default!;
        public IList<LeaveBalance> LeaveBalances { get; set; } = default!;
        public int? SelectedEmployeeId { get; set; }
        public int CurrentYear { get; set; } = DateTime.Now.Year;
        public ApplicationUser? CurrentUser { get; set; }
        public bool IsAdmin { get; set; }

        public async Task OnGetAsync(int? employeeId, int? year)
        {
            CurrentYear = year ?? DateTime.Now.Year;
            CurrentUser = await _userManager.GetUserAsync(User);
            IsAdmin = CurrentUser?.Role == "System Admin" || CurrentUser?.Role == "HR Admin";

            if (IsAdmin)
            {
                // Admins can see all employees and select any employee
                SelectedEmployeeId = employeeId;
                
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
            else
            {
                // Non-admin users can only see their own leave balance
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Email == CurrentUser!.Email && e.ActiveStatus);
                
                if (employee != null)
                {
                    SelectedEmployeeId = employee.EmployeeID;
                    LeaveBalances = await _leaveBalanceService.GetLeaveBalancesForEmployeeAsync(employee.EmployeeID);
                }
                else
                {
                    LeaveBalances = new List<LeaveBalance>();
                }
                
                Employees = new List<Employee>();
            }
        }
    }
}
