using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Entities
{
    public class MasterEmployee
    {
        public string ValueId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeLocation { get; set; }
        public string Roles { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
