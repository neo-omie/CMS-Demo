using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.Contracts.Queries.GetAllContracts;
using MediatR;

namespace CMS.Application.Features.Contracts.Queries.GetTerminatedContracts
{
    public class GetTerminatedContractsQueryHandler : IRequestHandler<GetTerminatedContractsQuery, IEnumerable<GetAllContractsDto>>
    {
        readonly IContractRepository _contractRepository;
        public GetTerminatedContractsQueryHandler(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }
        public async Task<IEnumerable<GetAllContractsDto>> Handle(GetTerminatedContractsQuery request, CancellationToken cancellationToken)
        {
            return await _contractRepository.GetTerminatedContractsAsync(request.pageNumber, request.pageSize);
        }
    }
}
