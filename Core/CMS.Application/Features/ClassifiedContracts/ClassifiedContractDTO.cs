using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Constants;

namespace CMS.Application.Features.ClassifiedContracts
{
    public class ClassifiedContractDTO
    {
        public string ClassifiedContractName { get; set; }
        public int DepartmentId { get; set; }
        public int ContractWithCompanyId { get; set; }
        public int ContractTypeId { get; set; }
        public int ApostilleTypeId { get; set; }
        public int ActualDocRefNo { get; set; }
        public RetainerType RetainerContract { get; set; }
        public string TermsAndConditions { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTill { get; set; }
        public DateTime? RenewalFrom { get; set; }
        public DateTime? RenewalTill { get; set; }
        public DateTime? AddendumDate { get; set; }
        //Property for uploading documents here
        public int EmpCustodianId { get; set; }
        public string Location { get; set; }
        public ContractStatus Approver1Status { get; set; } = ContractStatus.PendingApproval;
        public ContractStatus Approver2Status { get; set; } = ContractStatus.PendingApproval;
        public ContractStatus Approver3Status { get; set; } = ContractStatus.PendingApproval;
        public bool IsDeleted { get; set; } = false;
    }
}
