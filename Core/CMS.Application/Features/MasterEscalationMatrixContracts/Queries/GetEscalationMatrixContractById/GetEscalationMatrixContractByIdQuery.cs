using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CMS.Application.Features.MasterEscalationMatrixContracts.Queries.GetEscalationMatrixContractById
{
    public record GetEscalationMatrixContractByIdQuery(int id): IRequest<GetEscalationMatrixContractDto>;
    
}
