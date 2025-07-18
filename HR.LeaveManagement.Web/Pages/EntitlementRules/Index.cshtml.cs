using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Pages.EntitlementRules
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<EntitlementRule> EntitlementRules { get; set; } = default!;
        public int? LeaveTypeId { get; set; }
        public string? LeaveTypeName { get; set; }

        public async Task OnGetAsync(int? leaveTypeId)
        {
            LeaveTypeId = leaveTypeId;

            var query = _context.EntitlementRules
                .Include(er => er.LeaveType)
                .AsQueryable();

            if (leaveTypeId.HasValue)
            {
                query = query.Where(er => er.LeaveTypeID == leaveTypeId.Value);
                var leaveType = await _context.LeaveTypes.FindAsync(leaveTypeId.Value);
                LeaveTypeName = leaveType?.Name;
            }

            EntitlementRules = await query
                .OrderBy(er => er.LeaveType.Name)
                .ThenBy(er => er.Condition)
                .ToListAsync();
        }
    }
}
