using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CMS.Application.Features.Contracts.Commands.EditContract
{
    public record EditContractCommand(int id, ContractDTO cont) : IRequest<bool>;
}
