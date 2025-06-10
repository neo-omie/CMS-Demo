namespace CMS.Domain.Entities
{
    public class UserRoleMapping
    {
        public int UserRoleId { get; set; }
        public int RoleId { get; set; }
        public string EmployeeCode { get; set; }
        public Role Role { get; set; }
        public MasterEmployee Employee { get; set; }
    }
}
