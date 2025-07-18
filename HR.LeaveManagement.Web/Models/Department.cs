using System.ComponentModel.DataAnnotations;

namespace HR.LeaveManagement.Web.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        
        public int? ManagerID { get; set; }
        
        // Navigation properties
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
