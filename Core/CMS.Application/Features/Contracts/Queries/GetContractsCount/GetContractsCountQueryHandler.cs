using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Contracts.Queries.GetContractsCount
{
    public class GetContractsCountQueryHandler : IRequestHandler<GetContractsCountQuery, ContractsCount>
    {
        readonly IContractRepository _contractRepository;
        public GetContractsCountQueryHandler(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }
        public async Task<ContractsCount> Handle(GetContractsCountQuery request, CancellationToken cancellationToken)
        {
            return await _contractRepository.GetContractsCountAsync();
        }
    }
}
