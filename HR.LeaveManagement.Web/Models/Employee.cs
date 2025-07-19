namespace HR.LeaveManagement.Web.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public int? DepartmentID { get; set; }
        public DateTime JoinDate { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool ActiveStatus { get; set; } = true;
        
        // Navigation properties
        public virtual Department? DepartmentNavigation { get; set; }
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    }
}
