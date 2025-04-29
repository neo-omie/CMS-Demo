namespace CMS.Application.Features.EscalationMatrixMouMaster.Commands.UpdateEscalationMatrixMou
{
    public class UpdateEscalationMatrixMouDto
    {
        public string EscalationId1 { get; set; }
        public string EscalationId2 { get; set; }
        public string EscalationId3 { get; set; }

        public int TriggerDaysEscalation1 { get; set; }
        public int TriggerDaysEscalation2 { get; set; }
        public int TriggerDaysEscalation3 { get; set; }
    }
}
