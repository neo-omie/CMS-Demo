using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Application.Features.MasterEscalationMatrixContracts
{
    public class MatrixContractresponse
    {
        public IEnumerable<GetEscalationMatrixContractDto> getEscalationMatrixContractDto { get; set; }


        public int TotalCount { get; set; }
    }
}
