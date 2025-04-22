using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOU
{
    public record GetAllApprovalMatrixMOUQuery(int pageNumber, int pageSize) : IRequest<IEnumerable<GetAllApprovalMatrixMOUDto>>;
}
