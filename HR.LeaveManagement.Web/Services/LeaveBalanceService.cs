using HR.LeaveManagement.Web.Data;
using HR.LeaveManagement.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Services
{
    public class LeaveBalanceService
    {
        private readonly ApplicationDbContext _context;

        public LeaveBalanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LeaveBalance>> GetLeaveBalancesForEmployeeAsync(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
                return new List<LeaveBalance>();

            var leaveTypes = await _context.LeaveTypes
                .Include(lt => lt.EntitlementRules)
                .ToListAsync();

            var leaveBalances = new List<LeaveBalance>();
            var currentYear = DateTime.Now.Year;

            foreach (var leaveType in leaveTypes)
            {
                var entitledDays = CalculateEntitledDays(employee, leaveType);
                var usedDays = await CalculateUsedDaysAsync(employeeId, leaveType.LeaveTypeID, currentYear);
                var remainingDays = entitledDays - usedDays;

                leaveBalances.Add(new LeaveBalance
                {
                    LeaveTypeId = leaveType.LeaveTypeID,
                    LeaveTypeName = leaveType.Name,
                    EntitledDays = entitledDays,
                    UsedDays = usedDays,
                    RemainingDays = remainingDays,
                    Year = currentYear
                });
            }

            return leaveBalances;
        }

        private int CalculateEntitledDays(Employee employee, LeaveType leaveType)
        {
            var yearsOfService = CalculateYearsOfService(employee.JoinDate);
            
            // Find the most applicable entitlement rule
            var applicableRule = leaveType.EntitlementRules
                .Where(rule => EvaluateCondition(rule.Condition, employee, yearsOfService))
                .OrderByDescending(rule => GetConditionPriority(rule.Condition))
                .FirstOrDefault();

            return applicableRule?.EntitledDays ?? 0;
        }

        private int CalculateYearsOfService(DateTime joinDate)
        {
            var today = DateTime.Today;
            var years = today.Year - joinDate.Year;
            if (joinDate.Date > today.AddYears(-years))
                years--;
            return years;
        }

        private bool EvaluateCondition(string condition, Employee employee, int yearsOfService)
        {
            // Simple condition evaluation - in production, use a proper expression evaluator
            try
            {
                if (condition.Contains("YearsOfService"))
                {
                    var parts = condition.Split(new[] { ">=", "<=", "==", ">", "<" }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        var value = int.Parse(parts[1].Trim());
                        
                        if (condition.Contains(">="))
                            return yearsOfService >= value;
                        else if (condition.Contains("<="))
                            return yearsOfService <= value;
                        else if (condition.Contains("=="))
                            return yearsOfService == value;
                        else if (condition.Contains(">"))
                            return yearsOfService > value;
                        else if (condition.Contains("<"))
                            return yearsOfService < value;
                    }
                }
                
                // Add more condition types as needed
                return true; // Default to true if condition cannot be evaluated
            }
            catch
            {
                return false;
            }
        }

        private int GetConditionPriority(string condition)
        {
            // Higher priority for more specific conditions
            if (condition.Contains(">="))
            {
                var parts = condition.Split(">=");
                if (parts.Length == 2 && int.TryParse(parts[1].Trim(), out int value))
                    return value;
            }
            return 0;
        }

        private async Task<int> CalculateUsedDaysAsync(int employeeId, int leaveTypeId, int year)
        {
            var usedDays = await _context.LeaveRequests
                .Where(lr => lr.EmployeeID == employeeId 
                           && lr.LeaveTypeID == leaveTypeId 
                           && lr.FromDate.Year == year
                           && lr.Status == "Approved")
                .SumAsync(lr => lr.Duration);

            return usedDays;
        }
    }

    public class LeaveBalance
    {
        public int LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; } = string.Empty;
        public int EntitledDays { get; set; }
        public int UsedDays { get; set; }
        public int RemainingDays { get; set; }
        public int Year { get; set; }
    }
}
