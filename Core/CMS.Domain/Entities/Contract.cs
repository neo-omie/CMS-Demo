using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Constants;
using CMS.Domain.Entities.CompanyMaster;

namespace CMS.Domain.Entities
{
    public class Contract
    {
        [Key]
        public int ContractId { get; set; }
        public string ContractName { get; set; }
        public int DepartmentId { get; set; }
        public MasterApprovalMatrixContract Department { get; set; }
        public int ContractWithCompanyId { get; set; }
        public MasterCompany ContractWithCompany { get; set; }
        public int ContractTypeId { get; set; }
        public ContractTypeMasters ContractType { get; set; }
        public int ApostilleTypeId { get; set; }
        public MasterApostille ApostilleType { get; set; }
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
        public MasterEmployee EmpCustodian { get; set; }
        public string Location { get; set; }
        public ContractStatus Approver1Status { get; set; } = ContractStatus.PendingApproval;
        public ContractStatus Approver2Status { get; set; } = ContractStatus.PendingApproval;
        public ContractStatus Approver3Status { get; set; } = ContractStatus.PendingApproval;

    }
}
