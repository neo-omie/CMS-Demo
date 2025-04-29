using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Contracts.Commands.CreateNewContract
{
    public record CreateNewContractCommand(ContractDTO cont) : IRequest<Contract>;
}
