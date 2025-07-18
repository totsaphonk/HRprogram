using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Services
{
    public class DataSeedingService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeedingService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedDefaultAdminAsync()
        {
            // Ensure database is created
            await _context.Database.EnsureCreatedAsync();

            // Ensure roles exist
            string[] roles = { "System Admin", "HR Admin", "Line Manager", "Employee" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Check if admin user already exists
            var adminUser = await _userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                // Create default admin user
                adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@hrleave.com",
                    EmailConfirmed = true,
                    FullName = "System Administrator",
                    Department = "IT",
                    Role = "System Admin",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "System Admin");
                }
            }

            // Create sample HR user
            var hrUser = await _userManager.FindByNameAsync("hr.admin");
            if (hrUser == null)
            {
                hrUser = new ApplicationUser
                {
                    UserName = "hr.admin",
                    Email = "hr@hrleave.com",
                    EmailConfirmed = true,
                    FullName = "HR Administrator",
                    Department = "Human Resources",
                    Role = "HR Admin",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(hrUser, "Hr123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(hrUser, "HR Admin");
                }
            }

            // Create sample employee
            var employee = await _userManager.FindByNameAsync("john.doe");
            if (employee == null)
            {
                employee = new ApplicationUser
                {
                    UserName = "john.doe",
                    Email = "john.doe@hrleave.com",
                    EmailConfirmed = true,
                    FullName = "John Doe",
                    Department = "IT",
                    Role = "Employee",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(employee, "Employee123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(employee, "Employee");
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
