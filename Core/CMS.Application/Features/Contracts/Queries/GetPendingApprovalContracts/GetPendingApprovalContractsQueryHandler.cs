using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.Contracts.Queries.GetAllContracts;
using MediatR;

namespace CMS.Application.Features.Contracts.Queries.GetPendingApprovalContracts
{
    public class GetPendingApprovalContractsQueryHandler : IRequestHandler<GetPendingApprovalContractsQuery, IEnumerable<GetAllContractsDto>>
    {
        readonly IContractRepository _contractRepository;
        public GetPendingApprovalContractsQueryHandler(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }
        public async Task<IEnumerable<GetAllContractsDto>> Handle(GetPendingApprovalContractsQuery request, CancellationToken cancellationToken)
        {
            return await _contractRepository.GetPendingApprovalContractsAsync(request.pageNumber, request.pageSize);
        }
    }
}
