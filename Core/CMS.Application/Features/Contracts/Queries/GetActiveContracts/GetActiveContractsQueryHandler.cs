using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.Contracts.Queries.GetAllContracts;
using MediatR;

namespace CMS.Application.Features.Contracts.Queries.GetActiveContracts
{
    public class GetActiveContractsQueryHandler : IRequestHandler<GetActiveContractsQuery, IEnumerable<GetAllContractsDto>>
    {
        readonly IContractRepository _contractRepository;
        public GetActiveContractsQueryHandler(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }
        public async Task<IEnumerable<GetAllContractsDto>> Handle(GetActiveContractsQuery request, CancellationToken cancellationToken)
        {
            return await _contractRepository.GetActiveContractsAsync(request.pageNumber, request.pageSize);
        }
    }
}
