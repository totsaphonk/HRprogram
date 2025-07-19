using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HR.LeaveManagement.Web.Pages.Admin.Users
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public new ApplicationUser User { get; set; } = new ApplicationUser();
        public List<Department> Departments { get; set; } = new List<Department>();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            User = user;
            Input = new InputModel
            {
                Id = user.Id,
                FullName = user.FullName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                UserName = user.UserName ?? string.Empty,
                Role = user.Role ?? string.Empty,
                Department = user.Department ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive
            };

            await LoadDepartments();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadDepartments();

            if (!ModelState.IsValid)
            {
                User = await _context.Users.FindAsync(Input.Id) ?? new ApplicationUser();
                return Page();
            }

            var user = await _context.Users.FindAsync(Input.Id);
            if (user == null)
            {
                return NotFound();
            }

            // Check if email changed and if new email exists
            if (user.Email != Input.Email)
            {
                var existingUser = await _userManager.FindByEmailAsync(Input.Email);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    ModelState.AddModelError(string.Empty, "A user with this email already exists.");
                    User = user;
                    return Page();
                }
            }

            // Check if username changed and if new username exists
            if (user.UserName != Input.UserName)
            {
                var existingUser = await _userManager.FindByNameAsync(Input.UserName);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    ModelState.AddModelError(string.Empty, "A user with this username already exists.");
                    User = user;
                    return Page();
                }
            }

            user.FullName = Input.FullName;
            user.Email = Input.Email;
            user.UserName = Input.UserName;
            user.Role = Input.Role;
            user.Department = Input.Department;
            user.PhoneNumber = Input.PhoneNumber;
            user.IsActive = Input.IsActive;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"User '{user.FullName}' has been updated successfully.";
                return RedirectToPage("/Admin/Users");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            User = user;
            return Page();
        }

        private async Task LoadDepartments()
        {
            Departments = await _context.Departments
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        public class InputModel
        {
            public string Id { get; set; } = string.Empty;

            [Required]
            [Display(Name = "Full Name")]
            public string FullName { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; } = string.Empty;

            [Required]
            [Display(Name = "Username")]
            public string UserName { get; set; } = string.Empty;

            [Required]
            [Display(Name = "Role")]
            public string Role { get; set; } = string.Empty;

            [Required]
            [Display(Name = "Department")]
            public string Department { get; set; } = string.Empty;

            [Phone]
            [Display(Name = "Phone Number")]
            public string? PhoneNumber { get; set; }

            [Display(Name = "Active")]
            public bool IsActive { get; set; } = true;
        }
    }
}
