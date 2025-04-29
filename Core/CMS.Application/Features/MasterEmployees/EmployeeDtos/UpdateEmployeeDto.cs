
namespace CMS.Application.Features.MasterEmployees.EmployeeDtos
{
    public class UpdateEmployeeDto
    {
        public string EmployeeName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string EmployeeCode { get; set; }
        public string Unit { get; set; }
        public int DepartmentId { get; set; }
        public long EmployeeMobile { get; set; }
        public string Email { get; set; }
        public string EmployeeExtension { get; set; }
    }
}
