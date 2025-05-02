namespace CMS.Application.Features.MasterEscalationMatrixContracts
{
    public class GetEscalationMatrixContractDto
    {
        public int MatrixContractId { get; set; }
        public string Escalation1 { get; set; }
        public string Escalation2 { get; set; }
        public string Escalation3 { get; set; }
        public string EscalationId1 { get; set; }
        public string EscalationId2 { get; set; }
        public string EscalationId3 { get; set; }
        public int TriggerDaysEscalation1 { get; set; }
        public int TriggerDaysEscalation2 { get; set; }
        public int TriggerDaysEscalation3 { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int totalRecords { get; set; }

    }
}
