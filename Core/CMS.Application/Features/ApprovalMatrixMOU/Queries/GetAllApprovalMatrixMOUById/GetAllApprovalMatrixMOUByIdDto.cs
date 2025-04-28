using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOUById
{
    public class GetAllApprovalMatrixMOUByIdDto
    {
        public int MasterApprovalMatrixMOUId { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string ApproverName1 { get; set; }
        public string ApproverId1 { get; set; }
        public string ApproverName2 { get; set; }
        public string ApproverId2 { get; set; }
        public string ApproverName3 { get; set; }
        public string ApproverId3 { get; set; }
        public int NumberOfDays { get; set; }
    }
}
