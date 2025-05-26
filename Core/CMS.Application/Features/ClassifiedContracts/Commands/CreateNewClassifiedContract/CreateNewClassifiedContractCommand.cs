using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.ClassifiedContracts.Commands.CreateNewClassifiedContract
{
    public record CreateNewClassifiedContractCommand(ClassifiedContractDTO cont,string empName) : IRequest<ClassifiedContract>;
}
