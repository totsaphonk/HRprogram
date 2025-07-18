using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Pages.EntitlementRules
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EntitlementRule EntitlementRule { get; set; } = default!;

        public IList<LeaveType> LeaveTypes { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? leaveTypeId)
        {
            EntitlementRule = new EntitlementRule();
            
            if (leaveTypeId.HasValue)
            {
                EntitlementRule.LeaveTypeID = leaveTypeId.Value;
            }

            LeaveTypes = await _context.LeaveTypes
                .OrderBy(lt => lt.Name)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                LeaveTypes = await _context.LeaveTypes
                    .OrderBy(lt => lt.Name)
                    .ToListAsync();
                return Page();
            }

            _context.EntitlementRules.Add(EntitlementRule);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
