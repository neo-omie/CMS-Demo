using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOU
{
    public class GetAllApprovalMatrixMOUDto
    {
        public int MasterApprovalMatrixMOUId { get; set; }
        public string DepartmentName { get; set; }
        public string ApproverName1 { get; set; }
        public string ApproverName2 { get; set; }
        public string ApproverName3 { get; set; }
        public int TotalRecords { get; set; }
    }
}
