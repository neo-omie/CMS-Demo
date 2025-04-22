namespace CMS.Application.Features.ApprovalMatrixContract.Queries.GetAllApprovalMatrixContract
{
    public class GetAllApprovalMatrixContractDTO
    {
        public int MasterApprovalMatrixContractId { get; set; }
        public string DepartmentName { get; set; }
        public string ApproverName1 { get; set; }
        public string ApproverName2 { get; set; }
        public string ApproverName3 { get; set; }
        public int TotalRecords { get; set; }
    }
}
