using System.ComponentModel.DataAnnotations;

namespace HR.LeaveManagement.Web.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Department { get; set; } = string.Empty;
        
        [Required]
        public DateTime JoinDate { get; set; }
        
        [Required]
        [StringLength(30)]
        public string Role { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        
        public bool ActiveStatus { get; set; } = true;
        
        // Navigation properties
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    }
}
