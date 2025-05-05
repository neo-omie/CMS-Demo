using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Constants;

namespace CMS.Application.Features.ClassifiedContracts.Queries.GetAllClassifiedContracts
{
    public class GetAllClassifiedContractsDto
    {
        public int ClassifiedContractID { get; set; }
        public string ClassifiedContractName { get; set; }
        public string ContractType { get; set; }
        public string DepartmentName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime? ToBeRenewedOn { get; set; }
        public DateTime? AddendumDate { get; set; }
        public ContractStatus Status { get; set; }
        public string ApprovalPendingFrom { get; set; }
        public string RenewalContractPerson { get; set; }
        public string? RenewalDueIn{ get; set; }
        public string Location { get; set; }
        public int TotalRecords { get; set; }
    }
}
