using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Constants;
using CMS.Domain.Entities.CompanyMaster;

namespace CMS.Domain.Entities
{
    public class AddendumContract
    {
        public int DepartmentId { get; set; }
        public MasterApprovalMatrixContract Department { get; set; }
        public string ContractWithCompanyId { get; set; }
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

        public int EmpCustodianId { get; set; }
        public MasterEmployee EmpCustodian { get; set; }

        public string CompanyName { get; set; }
        public MasterCompany MasterCompany { get; set; }

        public string PocName { get; set; }

        public bool IsDeleted { get; set; }

    }
}
