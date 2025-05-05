using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CMS.Application.Features.ClassifiedContracts.Commands.EditClassifiedContract
{
    public record EditClassifiedContractCommand(int id, ClassifiedContractDTO cont) : IRequest<bool>;
}
