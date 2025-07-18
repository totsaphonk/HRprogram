using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR.LeaveManagement.Web.Models
{
    public class LeaveRequest
    {
        [Key]
        public int RequestID { get; set; }
        
        [Required]
        public int EmployeeID { get; set; }
        
        [Required]
        public int LeaveTypeID { get; set; }
        
        [Required]
        public DateTime FromDate { get; set; }
        
        [Required]
        public DateTime ToDate { get; set; }
        
        [Required]
        public int Duration { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Reason { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        public int? ApprovedBy { get; set; }
        
        [StringLength(500)]
        public string Comments { get; set; } = string.Empty;
        
        // Navigation properties
        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; } = null!;
        
        [ForeignKey("LeaveTypeID")]
        public virtual LeaveType LeaveType { get; set; } = null!;
    }
}
