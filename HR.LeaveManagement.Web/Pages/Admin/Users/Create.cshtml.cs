using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HR.LeaveManagement.Web.Pages.Admin.Users
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public List<Department> Departments { get; set; } = new List<Department>();

        public async Task OnGetAsync()
        {
            await LoadDepartments();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadDepartments();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check if email already exists
            var existingUser = await _userManager.FindByEmailAsync(Input.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "A user with this email already exists.");
                return Page();
            }

            // Check if username already exists
            existingUser = await _userManager.FindByNameAsync(Input.UserName);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "A user with this username already exists.");
                return Page();
            }

            var user = new ApplicationUser
            {
                UserName = Input.UserName,
                Email = Input.Email,
                FullName = Input.FullName,
                Role = Input.Role,
                Department = Input.Department,
                PhoneNumber = Input.PhoneNumber,
                IsActive = Input.IsActive,
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true // Auto-confirm email for admin-created users
            };

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"User '{user.FullName}' has been created successfully.";
                return RedirectToPage("/Admin/Users");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

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

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;

            [Display(Name = "Active")]
            public bool IsActive { get; set; } = true;
        }
    }
}
