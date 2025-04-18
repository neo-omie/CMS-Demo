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
        public string Department { get; set; }
        public string EmployeeMobile { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeExtension { get; set; }
        public List<string> Roles { get; set; }
    }
}
