using System.ComponentModel.DataAnnotations;

namespace CMS.Domain.Entities
{
    public class UserRoleMapping
    {
        [Key]
        public int UserRoleId { get; set; }
        public string EmployeeCode { get; set; }
        public int RoleId { get; set; }
    }
}
