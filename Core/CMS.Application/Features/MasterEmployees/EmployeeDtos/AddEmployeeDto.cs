namespace CMS.Application.Features.MasterEmployees.EmployeeDtos
{
    public class AddEmployeeDto
    {
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string Unit { get; set; }
        public int DepartmentId { get; set; }
        public string EmployeeMobile { get; set; }
        public string Email { get; set; }
        public string EmployeeExtension { get; set; }
    }
}
