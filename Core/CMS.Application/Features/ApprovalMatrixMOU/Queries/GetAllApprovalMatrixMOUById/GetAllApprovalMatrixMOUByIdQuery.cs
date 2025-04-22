using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOUById
{
    public record GetAllApprovalMatrixMOUByIdQuery(int id) : IRequest<GetAllApprovalMatrixMOUByIdDto>;
}
