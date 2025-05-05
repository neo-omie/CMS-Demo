using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterEmployees.EmployeeDtos
{
    public class GetEmployeesByDepartmentIdAndEmpDetailsDto
    {
        public string ValueId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
    }
}
