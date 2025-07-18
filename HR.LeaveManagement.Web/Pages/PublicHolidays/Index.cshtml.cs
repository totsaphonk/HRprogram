using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Pages.PublicHolidays
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PublicHoliday> PublicHolidays { get; set; } = default!;
        public int SelectedYear { get; set; } = DateTime.Now.Year;
        public string? RegionFilter { get; set; }
        public bool RecurringFilter { get; set; }

        public async Task OnGetAsync(int? year, string? region, bool? recurringOnly)
        {
            SelectedYear = year ?? DateTime.Now.Year;
            RegionFilter = region;
            RecurringFilter = recurringOnly ?? false;

            var query = _context.PublicHolidays.AsQueryable();

            // Filter by year
            query = query.Where(h => h.Date.Year == SelectedYear);

            // Filter by region
            if (!string.IsNullOrEmpty(region))
            {
                query = query.Where(h => h.Region == region);
            }

            // Filter by recurring
            if (RecurringFilter)
            {
                query = query.Where(h => h.IsRecurring);
            }

            PublicHolidays = await query
                .OrderBy(h => h.Date)
                .ToListAsync();
        }
    }
}
