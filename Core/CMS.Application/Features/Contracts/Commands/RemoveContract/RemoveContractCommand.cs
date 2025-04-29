using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CMS.Application.Features.Contracts.Commands.RemoveContract
{
    public record RemoveContractCommand(int id) : IRequest<bool>;
}
