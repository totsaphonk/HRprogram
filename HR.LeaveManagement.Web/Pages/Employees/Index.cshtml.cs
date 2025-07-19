using HR.LeaveManagement.Web.Models;
using HR.LeaveManagement.Web.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Pages.Employees
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Employee> Employees { get; set; }

        public async Task OnGetAsync()
        {
            Employees = await _context.Employees.ToListAsync();
        }
    }
}