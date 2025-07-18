using System.ComponentModel.DataAnnotations;

namespace HR.LeaveManagement.Web.Models
{
    public class PublicHoliday
    {
        [Key]
        public int HolidayID { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public DateTime Date { get; set; }
        
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        
        public bool IsRecurring { get; set; } = false;
        
        [StringLength(50)]
        public string Region { get; set; } = "National";
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; }
    }
}
