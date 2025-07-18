using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR.LeaveManagement.Web.Models
{
    public class NotificationLog
    {
        [Key]
        public int LogID { get; set; }

        [Required]
        public int TemplateID { get; set; }

        [Required]
        [StringLength(100)]
        public string RecipientEmail { get; set; } = string.Empty;

        [StringLength(100)]
        public string RecipientName { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Body { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Sent, Failed

        public DateTime CreatedAt { get; set; }

        public DateTime? SentAt { get; set; }

        [StringLength(500)]
        public string ErrorMessage { get; set; } = string.Empty;

        public int? RelatedEntityID { get; set; } // LeaveRequest ID, etc.

        [StringLength(50)]
        public string RelatedEntityType { get; set; } = string.Empty; // LeaveRequest, Employee, etc.

        // Navigation properties
        [ForeignKey("TemplateID")]
        public virtual NotificationTemplate Template { get; set; } = null!;
    }
}
