using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.ApprovalMatrixMOU.Commands.UpdateApprovalMatrixMOU;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Departments.Commands.AddMOUApprovers
{
    public record AddMOUApproversCommand(int id, UpdateApprovalMatrixMOUDto addApprovers) : IRequest<MasterApprovalMatrixMOU>;
}
