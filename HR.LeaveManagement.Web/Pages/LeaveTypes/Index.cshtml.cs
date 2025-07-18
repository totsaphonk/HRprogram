using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Pages.LeaveTypes
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<LeaveType> LeaveTypes { get; set; } = default!;

        public async Task OnGetAsync()
        {
            LeaveTypes = await _context.LeaveTypes
                .Include(lt => lt.EntitlementRules)
                .OrderBy(lt => lt.Name)
                .ToListAsync();
        }
    }
}
