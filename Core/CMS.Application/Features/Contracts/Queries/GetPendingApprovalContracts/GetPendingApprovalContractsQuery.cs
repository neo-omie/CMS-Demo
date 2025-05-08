using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.Contracts.Queries.GetAllContracts;
using MediatR;

namespace CMS.Application.Features.Contracts.Queries.GetPendingApprovalContracts
{
    public record GetPendingApprovalContractsQuery(int pageNumber, int pageSize) : IRequest<IEnumerable<GetAllContractsDto>>;
}
