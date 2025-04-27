using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.ApprovalMatrixMOU.Commands.UpdateApprovalMatrixMOU
{
    public record UpdateApprovalMatrixMOUCommand(int id, UpdateApprovalMatrixMOUDto mou) : IRequest<bool>;
}
