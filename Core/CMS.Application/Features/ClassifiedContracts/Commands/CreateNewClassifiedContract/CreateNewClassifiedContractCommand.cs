using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.ClassifiedContracts.Commands.CreateNewContract
{
    public record CreateNewClassifiedContractCommand(ClassifiedContractDTO cont) : IRequest<ClassifiedContract>;
}
