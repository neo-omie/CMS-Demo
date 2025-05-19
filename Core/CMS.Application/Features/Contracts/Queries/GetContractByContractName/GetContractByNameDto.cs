

using CMS.Domain.Constants;

namespace CMS.Application.Features.Contracts.Queries.GetContractByContractName
{
    public class GetContractByNameDto
    {
        public int AddendumContractId { get; set; }
        public int ContractId { get; set; }
        public string ContractName { get; set; }
        public int DepartmentId { get; set; }
        public int ContractWithCompanyId {  get; set; }

        public int ContractTypeId { get; set; }
        public int ApostilleTypeId { get; set; }
        public int ActualDocRefNo { get;set; }
        public RetainerType RetainerContract { get; set; }
        public string TermsAndConditions { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTill { get; set; }

        public int EmpCustodianId { get; set; }
        public bool IsDeleted { get; set; }

    }
}
