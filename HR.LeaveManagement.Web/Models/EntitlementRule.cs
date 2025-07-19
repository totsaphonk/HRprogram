using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR.LeaveManagement.Web.Models
{
    public class EntitlementRule
    {
        [Key]
        public int RuleID { get; set; }
        
        [Required]
        public int LeaveTypeID { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Condition { get; set; } = string.Empty;
        
        [Required]
        public int EntitledDays { get; set; }
        
        // Navigation properties
        [ForeignKey("LeaveTypeID")]
        public virtual LeaveType? LeaveType { get; set; }
    }
}
