using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.ContractTypeMaster
{
    public class GetContractDTO
    {
        public int ValueId { get; set; }

        public string ContractTypeName { get; set; }

        public bool Status { get; set; } = true;

    }
}
