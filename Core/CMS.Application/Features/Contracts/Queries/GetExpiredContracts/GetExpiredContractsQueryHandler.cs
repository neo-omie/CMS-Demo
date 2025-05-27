using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.Contracts.Queries.GetAllContracts;
using MediatR;

namespace CMS.Application.Features.Contracts.Queries.GetExpiredContracts
{
    internal class GetExpiredContractsQueryHandler : IRequestHandler<GetExpiredContractsQuery, IEnumerable<GetAllContractsDto>>
    {
        readonly IContractRepository _contractRepository;
        public GetExpiredContractsQueryHandler(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }
        public async Task<IEnumerable<GetAllContractsDto>> Handle(GetExpiredContractsQuery request, CancellationToken cancellationToken)
        {
            return await _contractRepository.GetExpiredContractsAsync(request.pageNumber, request.pageSize);
        }
    }
}
