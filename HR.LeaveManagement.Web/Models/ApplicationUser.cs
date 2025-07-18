using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HR.LeaveManagement.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string Department { get; set; } = string.Empty;
        
        public DateTime? JoinDate { get; set; }
        
        [StringLength(30)]
        public string Role { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? LastLoginAt { get; set; }
    }
}
