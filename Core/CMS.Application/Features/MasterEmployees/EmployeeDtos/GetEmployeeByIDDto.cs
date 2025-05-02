using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterEmployees.EmployeeDtos
{
    public class GetEmployeeByIDDto
    {
        public int ValueId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string EmployeeName { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string EmployeeCode { get; set; }
        public string Unit { get; set; }
        public long EmployeeMobile { get; set; }
        public string EmployeeExtension { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime LastPasswordChanged { get; set; }
    }
}
