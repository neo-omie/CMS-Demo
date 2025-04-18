using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CMS.Domain.Entities
{
    public class MasterEmployee : IdentityUser
    {
        public int ValueId { get; set; }
        public string EmployeeName { get; set; }
        //public string EmployeeLocation { get; set; }
        //public string Roles { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string EmployeeCode { get; set; }
        public string Unit { get; set; }
        //public string Department { get; set; }
        public long EmployeeMobile { get; set; }
        public string EmployeeExtension { get; set; }

        public int DepartmentId { get; set; }
        public DateTime LastPasswordChanged { get; set; }



    }
}
