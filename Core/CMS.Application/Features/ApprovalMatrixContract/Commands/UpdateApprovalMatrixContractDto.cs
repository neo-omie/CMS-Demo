using System.ComponentModel.DataAnnotations;

namespace CMS.Application.Features.ApprovalMatrixContract.Commands
{
    public class UpdateApprovalMatrixContractDto
    {
        public string ApproverId1 { get; set; }
        public string ApproverId2 { get; set; }
        public string ApproverId3 { get; set; }
        public int NumberOfDays { get; set; }
    }
}
