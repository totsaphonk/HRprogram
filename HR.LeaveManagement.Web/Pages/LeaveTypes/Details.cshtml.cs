using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Pages.LeaveTypes
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public LeaveType LeaveType { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _context.LeaveTypes
                .Include(lt => lt.EntitlementRules)
                .FirstOrDefaultAsync(m => m.LeaveTypeID == id);

            if (leaveType == null)
            {
                return NotFound();
            }

            LeaveType = leaveType;
            return Page();
        }
    }
}
