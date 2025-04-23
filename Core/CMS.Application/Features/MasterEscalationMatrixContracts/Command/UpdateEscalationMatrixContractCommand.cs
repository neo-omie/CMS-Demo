using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CMS.Application.Features.MasterEscalationMatrixContracts.Command
{
    public record UpdateEscalationMatrixContractCommand(int id,UpdateEscalationMatrixContractDto updateDto) : IRequest<int>;
    
    
}
