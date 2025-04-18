using System.ComponentModel.DataAnnotations;

namespace CMS.Domain.Entities
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        [Required]
        public string DepartmentName { get; set; }
    }
}
