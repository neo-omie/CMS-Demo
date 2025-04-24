namespace CMS.Application.Features.ApprovalMatrixContract.Queries.GetApprovalMatrixContractById
{
    public class GetApprovalMatrixContractByIdDto
    {
        public int MasterApprovalMatrixContractId { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string ApproverName1 { get; set; }
        public string ApproverName2 { get; set; }
        public string ApproverName3 { get; set; }
        public int NumberOfDays {  get; set; }
    }
}
