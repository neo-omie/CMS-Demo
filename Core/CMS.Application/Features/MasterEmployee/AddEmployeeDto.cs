namespace CMS.Application.Features.MasterEmployee
{
    public class AddEmployeeDto
    {
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string Unit { get; set; }
        public string Department { get; set; }
        public int EmpMobile { get; set; }
        public string EmpEmail { get; set; }
        public string EmpExt { get; set; }
        public List<string> Roles { get; set; }
    }
}
