using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using HR.LeaveManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Pages.LeaveRequests
{
    public class ApplyModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public ApplyModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [BindProperty]
        public LeaveRequest LeaveRequest { get; set; } = default!;

        public IList<Employee> Employees { get; set; } = default!;
        public IList<LeaveType> LeaveTypes { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            LeaveRequest = new LeaveRequest
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                CreatedAt = DateTime.UtcNow,
                Status = "Pending"
            };

            Employees = await _context.Employees
                .Where(e => e.ActiveStatus)
                .OrderBy(e => e.FullName)
                .ToListAsync();

            LeaveTypes = await _context.LeaveTypes
                .OrderBy(lt => lt.Name)
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

                LeaveTypes = await _context.LeaveTypes
                    .OrderBy(lt => lt.Name)
                    .ToListAsync();

                return Page();
            }

            // Additional validation
            if (LeaveRequest.FromDate > LeaveRequest.ToDate)
            {
                ModelState.AddModelError("LeaveRequest.ToDate", "To Date must be after From Date");
                
                Employees = await _context.Employees
                    .Where(e => e.ActiveStatus)
                    .OrderBy(e => e.FullName)
                    .ToListAsync();

                LeaveTypes = await _context.LeaveTypes
                    .OrderBy(lt => lt.Name)
                    .ToListAsync();

                return Page();
            }

            // Set default values
            LeaveRequest.CreatedAt = DateTime.UtcNow;
            LeaveRequest.Status = "Pending";

            _context.LeaveRequests.Add(LeaveRequest);
            await _context.SaveChangesAsync();

            // Send notification to HR/Manager
            try
            {
                await _emailService.SendLeaveRequestNotificationAsync(LeaveRequest, "LeaveRequestSubmitted");
            }
            catch (Exception ex)
            {
                // Log error but don't prevent the user from continuing
                // In production, you might want to add proper logging
                Console.WriteLine($"Failed to send notification: {ex.Message}");
            }

            TempData["SuccessMessage"] = "Leave application submitted successfully! HR has been notified.";
            return RedirectToPage("./Index");
        }
    }
}
