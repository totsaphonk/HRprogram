using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;

namespace HR.LeaveManagement.Web.Pages.PublicHolidays
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public PublicHoliday PublicHoliday { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            PublicHoliday.CreatedAt = DateTime.UtcNow;
            _context.PublicHolidays.Add(PublicHoliday);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
