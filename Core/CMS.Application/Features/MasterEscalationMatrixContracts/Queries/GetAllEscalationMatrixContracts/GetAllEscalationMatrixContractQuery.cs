using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CMS.Application.Features.MasterEscalationMatrixContracts.Queries.GetAllEscalationMatrixContracts
{
    public record GetAllEscalationMatrixContractQuery(int pageNumber, int pageSize) : IRequest<(IEnumerable<GetEscalationMatrixContractDto>, int)>;
    
}
