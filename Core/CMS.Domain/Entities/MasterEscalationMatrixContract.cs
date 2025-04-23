using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Entities
{
    public class MasterEscalationMatrixContract
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MatrixContractId { get; set; }
 
        public string EscalationId1 { get; set; }
        public string EscalationId2 { get; set; }
        public string EscalationId3 { get; set; }
        public MasterEmployee Escalation1 { get; set; }
        public MasterEmployee Escalation2 { get; set; }
        public MasterEmployee Escalation3 { get; set; }


        public int TriggerDaysEscalation1 { get; set; }
        public int TriggerDaysEscalation2 { get; set; }
        public int TriggerDaysEscalation3 { get; set; }


        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        
    }
}
