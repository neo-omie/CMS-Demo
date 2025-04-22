using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Application.Features.EscalationMatrixContract
{
    public class GetEscalationMatrixContractDto
    {
        
        public int MatrixContractId { get; set; }

        public string Escalation1 { get; set; }
        public string Escalation2 { get; set; }
        public string Escalation3 { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
 