using CMS.Domain.Entities;

namespace CMS.Application.Features.EscalationMatrixMouMaster
{
    public class EscalationMatrixMoutDto
    {
        public int MatrixMouId { get; set; }
        public string Escalation1 { get; set; }
        public string Escalation2 { get; set; }
        public string Escalation3 { get; set; }
        public int TriggerDaysEscalation1 { get; set; }
        public int TriggerDaysEscalation2 { get; set; }
        public int TriggerDaysEscalation3 { get; set; }

        public string DepartmentName { get; set; }

        public int totalRecords { get; set; }
    }
}
