using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.Departments.Queries.GetAllDepartments
{
    public class GetAllDepartmentsDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int TotalRecords { get; set; }
    }
}
