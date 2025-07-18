using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using HR.LeaveManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Pages.LeaveRequests
{
    public class RejectModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public RejectModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [BindProperty]
        public LeaveRequest LeaveRequest { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveRequest = await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.LeaveType)
                .FirstOrDefaultAsync(m => m.RequestID == id);

            if (leaveRequest == null)
            {
                return NotFound();
            }

            if (leaveRequest.Status != "Pending")
            {
                TempData["ErrorMessage"] = "This leave request has already been processed.";
                return RedirectToPage("./Index");
            }

            LeaveRequest = leaveRequest;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(LeaveRequest.Comments))
            {
                ModelState.AddModelError("LeaveRequest.Comments", "Rejection reason is required.");
            }

            if (!ModelState.IsValid)
            {
                // Reload the leave request with related data
                LeaveRequest = await _context.LeaveRequests
                    .Include(lr => lr.Employee)
                    .Include(lr => lr.LeaveType)
                    .FirstOrDefaultAsync(m => m.RequestID == LeaveRequest.RequestID) ?? LeaveRequest;
                
                return Page();
            }

            var leaveRequest = await _context.LeaveRequests.FindAsync(LeaveRequest.RequestID);

            if (leaveRequest == null)
            {
                return NotFound();
            }

            if (leaveRequest.Status != "Pending")
            {
                TempData["ErrorMessage"] = "This leave request has already been processed.";
                return RedirectToPage("./Index");
            }

            // Update the leave request
            leaveRequest.Status = "Rejected";
            leaveRequest.Comments = LeaveRequest.Comments;
            leaveRequest.ApprovedBy = 1; // TODO: Get actual user ID from authentication

            try
            {
                await _context.SaveChangesAsync();
                
                // Send notification to employee
                try
                {
                    await _emailService.SendLeaveStatusNotificationAsync(leaveRequest, "Rejected", LeaveRequest.Comments);
                }
                catch (Exception ex)
                {
                    // Log error but don't prevent the user from continuing
                    Console.WriteLine($"Failed to send notification: {ex.Message}");
                }
                
                TempData["SuccessMessage"] = "Leave request has been rejected. Employee has been notified.";
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["ErrorMessage"] = "An error occurred while processing the request.";
            }

            return RedirectToPage("./Index");
        }
    }
}
