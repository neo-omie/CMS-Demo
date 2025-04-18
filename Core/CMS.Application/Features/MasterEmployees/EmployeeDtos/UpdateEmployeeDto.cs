using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterEmployees.EmployeeDtos
{
    public class UpdateEmployeeDto
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
