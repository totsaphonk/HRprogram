using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Pages.Departments
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Department Department { get; set; } = default!;

        public IList<Employee> Employees { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            Department = new Department();
            Employees = await _context.Employees
                .Where(e => e.ActiveStatus)
                .OrderBy(e => e.FullName)
                .ToListAsync();
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Employees = await _context.Employees
                    .Where(e => e.ActiveStatus)
                    .OrderBy(e => e.FullName)
                    .ToListAsync();
                return Page();
            }

            _context.Departments.Add(Department);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
