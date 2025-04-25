using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.MasterEscalationMatrixContracts.Command;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Departments.Commands.AddContractEscalators
{
    public record AddContractEscalatorsCommand(int id, UpdateEscalationMatrixContractDto addEscalators) : IRequest<MasterEscalationMatrixContract>;
}
