using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Pages.Departments
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Department Department { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (department == null)
            {
                return NotFound();
            }
            Department = department;
            
            // Get list of employees for manager selection
            ViewData["ManagerID"] = new SelectList(await _context.Employees
                .Where(e => e.ActiveStatus)
                .Select(e => new { e.EmployeeID, e.FullName })
                .ToListAsync(), "EmployeeID", "FullName", Department.ManagerID);
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Reload the manager dropdown if validation fails
                ViewData["ManagerID"] = new SelectList(await _context.Employees
                    .Where(e => e.ActiveStatus)
                    .Select(e => new { e.EmployeeID, e.FullName })
                    .ToListAsync(), "EmployeeID", "FullName", Department.ManagerID);
                return Page();
            }

            _context.Attach(Department).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Department updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(Department.DepartmentID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.DepartmentID == id);
        }
    }
}
