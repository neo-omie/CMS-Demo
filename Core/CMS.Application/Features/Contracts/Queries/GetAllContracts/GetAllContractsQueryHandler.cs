using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Contracts.Queries.GetAllContracts
{
    public class GetAllContractsQueryHandler : IRequestHandler<GetAllContractsQuery, IEnumerable<Contract>>
    {
        readonly IContractRepository _contractRepository;
        public GetAllContractsQueryHandler(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }
        public async Task<IEnumerable<Contract>> Handle(GetAllContractsQuery request, CancellationToken cancellationToken)
        {
            return await _contractRepository.GetAllContractsAsync(request.pageNumber, request.pageSize);
        }
    }
}
