using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HR.LeaveManagement.Web.Pages.LeaveTypes
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LeaveType LeaveType { get; set; } = default!;

        public IActionResult OnGet()
        {
            LeaveType = new LeaveType();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.LeaveTypes.Add(LeaveType);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
