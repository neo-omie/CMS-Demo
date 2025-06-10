namespace CMS.Domain.Entities
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public IList<UserRoleMapping> UserRoleMappings { get; set; }
    }
}
