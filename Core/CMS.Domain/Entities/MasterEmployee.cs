using System.ComponentModel.DataAnnotations;

namespace CMS.Domain.Entities
{
    public class MasterEmployee
    {
        [Key]
        public int ValueId { get; set; }
        [EmailAddress,Required]
        public string Email {  get; set; }
        public string Password { get; set; }
        public string Role {  get; set; }
        public string EmployeeName { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string EmployeeCode { get; set; }
        public string Unit { get; set; }
        public long EmployeeMobile { get; set; }
        public int EmployeeExtension { get; set; }
        public int DepartmentId { get; set; }
        public DateTime LastPasswordChanged { get; set; }
    }
}
