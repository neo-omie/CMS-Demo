using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.Contracts.Queries.GetAllContracts
{
    public class FiltersContractDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SearchTerm { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public int? ContractType { get; set; }
        public int? RenewalDueIn { get; set; }
        public int? ContractStatus { get; set; }
        public int? Department { get; set; }
        public string? Location { get; set; }
        public bool? HasAddendum { get; set; }
    }
}
