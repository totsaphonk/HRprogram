using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;

namespace HR.LeaveManagement.Web.Pages.Calendar
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public DateTime CurrentMonth { get; set; }
        public List<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
        public List<PublicHoliday> PublicHolidays { get; set; } = new List<PublicHoliday>();

        public async Task OnGetAsync(int? year, int? month)
        {
            // Set current month - default to current month if not specified
            if (year.HasValue && month.HasValue)
            {
                CurrentMonth = new DateTime(year.Value, month.Value, 1);
            }
            else
            {
                CurrentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }

            // Get the date range for the calendar view (including previous/next month days)
            var firstDay = CurrentMonth;
            var lastDay = firstDay.AddMonths(1).AddDays(-1);
            var startDate = firstDay.AddDays(-(int)firstDay.DayOfWeek);
            var endDate = lastDay.AddDays(6 - (int)lastDay.DayOfWeek);

            // Load leave requests that overlap with the calendar view
            LeaveRequests = await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.LeaveType)
                .Where(lr => lr.FromDate.Date <= endDate && lr.ToDate.Date >= startDate)
                .ToListAsync();

            // Load public holidays for the calendar view
            PublicHolidays = await _context.PublicHolidays
                .Where(ph => ph.Date.Date >= startDate && ph.Date.Date <= endDate)
                .ToListAsync();
        }
    }
}
