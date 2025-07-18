using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using System.Text;

namespace HR.LeaveManagement.Web.Pages.Calendar
{
    public class ExportModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ExportModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public DateTime CurrentMonth { get; set; }
        public List<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
        public List<PublicHoliday> PublicHolidays { get; set; } = new List<PublicHoliday>();
        public List<Department> Departments { get; set; } = new List<Department>();

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

            await LoadData();
        }

        public async Task<IActionResult> OnPostAsync(int year, int month, string format, bool includeLeaveRequests, bool includePublicHolidays, int? departmentId)
        {
            CurrentMonth = new DateTime(year, month, 1);
            await LoadData(departmentId);

            switch (format?.ToLower())
            {
                case "csv":
                    return await ExportToCsv(includeLeaveRequests, includePublicHolidays);
                case "excel":
                    return await ExportToExcel(includeLeaveRequests, includePublicHolidays);
                case "pdf":
                    return await ExportToPdf(includeLeaveRequests, includePublicHolidays);
                default:
                    return await ExportToCsv(includeLeaveRequests, includePublicHolidays);
            }
        }

        private async Task LoadData(int? departmentId = null)
        {
            var firstDay = CurrentMonth;
            var lastDay = firstDay.AddMonths(1).AddDays(-1);

            // Load leave requests
            var leaveQuery = _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.LeaveType)
                .Where(lr => lr.FromDate.Date <= lastDay && lr.ToDate.Date >= firstDay);

            if (departmentId.HasValue)
            {
                leaveQuery = leaveQuery.Where(lr => lr.Employee.Department == departmentId.Value.ToString());
            }

            LeaveRequests = await leaveQuery.ToListAsync();

            // Load public holidays
            PublicHolidays = await _context.PublicHolidays
                .Where(ph => ph.Date.Date >= firstDay && ph.Date.Date <= lastDay)
                .ToListAsync();

            // Load departments for filter
            Departments = await _context.Departments.ToListAsync();
        }

        private async Task<IActionResult> ExportToCsv(bool includeLeaveRequests, bool includePublicHolidays)
        {
            var csv = new StringBuilder();
            
            // Add header
            csv.AppendLine($"HR Leave Management Calendar Export - {CurrentMonth:MMMM yyyy}");
            csv.AppendLine();

            if (includeLeaveRequests && LeaveRequests.Any())
            {
                csv.AppendLine("Leave Requests");
                csv.AppendLine("Employee Name,Department,Leave Type,Start Date,End Date,Status,Days");
                
                foreach (var leave in LeaveRequests)
                {
                    var days = (leave.ToDate - leave.FromDate).Days + 1;
                    csv.AppendLine($"\"{leave.Employee.FullName}\",\"{leave.Employee.Department}\",\"{leave.LeaveType.Name}\",{leave.FromDate:yyyy-MM-dd},{leave.ToDate:yyyy-MM-dd},{leave.Status},{days}");
                }
                csv.AppendLine();
            }

            if (includePublicHolidays && PublicHolidays.Any())
            {
                csv.AppendLine("Public Holidays");
                csv.AppendLine("Name,Date");
                
                foreach (var holiday in PublicHolidays)
                {
                    csv.AppendLine($"\"{holiday.Name}\",{holiday.Date:yyyy-MM-dd}");
                }
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", $"Calendar_{CurrentMonth:yyyy_MM}.csv");
        }

        private Task<IActionResult> ExportToExcel(bool includeLeaveRequests, bool includePublicHolidays)
        {
            // For simplicity, we'll return CSV format for now
            // In a real application, you would use a library like EPPlus or ClosedXML
            return ExportToCsv(includeLeaveRequests, includePublicHolidays);
        }

        private Task<IActionResult> ExportToPdf(bool includeLeaveRequests, bool includePublicHolidays)
        {
            // For simplicity, we'll return CSV format for now
            // In a real application, you would use a library like iTextSharp or PdfSharp
            return ExportToCsv(includeLeaveRequests, includePublicHolidays);
        }
    }
}
