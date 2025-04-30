using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.ContractTypeMaster.Query
{
    public class GetAllContractTypesDTO
    {
        public int ValueId { get; set; }

        public string ContractTypeName { get; set; }

        public bool Status { get; set; } = true;

        //for soft delete
        public bool IsDeleted { get; set; } = false;
        public int TotalRecords { get; set; }
    }
}
