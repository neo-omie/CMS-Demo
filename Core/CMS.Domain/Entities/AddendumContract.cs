using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using CMS.Domain.Constants;
using CMS.Domain.Entities.CompanyMaster;

namespace CMS.Domain.Entities
{
    public class AddendumContract
    {

        //Contract Details 
        [Key]
        public int AddendumContractId { get; set; }

        [Required]
        public int ContractId { get; set; }
        public Contract Contract { get; set; }

        [Required]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [Required]
        public int ContractWithCompanyId { get; set; }
        public MasterCompany ContractWithCompany { get; set; }

        [Required]
        public int ContractTypeId { get; set; }
        public ContractTypeMasters ContractType { get; set; }

        [Required]
        public int ApostilleTypeId { get; set; }
        public MasterApostille ApostilleType { get; set; }

        public int ActualDocRefNo { get; set; } //This will be autofilled 

        [Required]
        public RetainerType RetainerContract { get; set; } 

        [Required]
        [MaxLength(200)]
        public string TermsAndConditions { get; set; }

        //Date From Contract 
        public DateTime ValidFrom { get; set; } //This will be autofilled 
        public DateTime ValidTill { get; set; } //This will be autofilled 

        //Employee Custodian Details
        public int EmpCustodianId { get; set; } //This will be autofilled 
        public MasterEmployee EmpCustodian { get; set; }
        public bool IsDeleted { get; set; } = false; //for soft delete
    }
}
