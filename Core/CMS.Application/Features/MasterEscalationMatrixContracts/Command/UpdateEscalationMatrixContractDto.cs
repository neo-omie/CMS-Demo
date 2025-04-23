using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterEscalationMatrixContracts.Command
{
    public class UpdateEscalationMatrixContractDto
    {
        public string EscalationId1 { get; set; }
        public string EscalationId2 { get; set; }
        public string EscalationId3 { get; set; }

        public int TriggerDaysEscalation1 { get; set; }
        public int TriggerDaysEscalation2 { get; set; }
        public int TriggerDaysEscalation3 { get; set; }

    }
}
