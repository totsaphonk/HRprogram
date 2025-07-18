using System.ComponentModel.DataAnnotations;

namespace HR.LeaveManagement.Web.Models
{
    public class LeaveType
    {
        public int LeaveTypeID { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        
        public bool RequiresDoc { get; set; } = false;
        
        public bool AllowHalfDay { get; set; } = false;
        
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        
        // Navigation properties
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
        public virtual ICollection<EntitlementRule> EntitlementRules { get; set; } = new List<EntitlementRule>();
    }
}
