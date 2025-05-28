using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.ClassifiedContracts.Queries.GetClassifiedContractsCount
{
    public class GetClassifiedContractsCountQueryHandler : IRequestHandler<GetClassifiedContractsCountQuery, ContractsCount>
    {
        readonly IClassifiedContractRepository _classifiedContractRepository;
        public GetClassifiedContractsCountQueryHandler(IClassifiedContractRepository classifiedContractRepository)
        {
            _classifiedContractRepository = classifiedContractRepository;
        }
        public async Task<ContractsCount> Handle(GetClassifiedContractsCountQuery request, CancellationToken cancellationToken)
        {
            return await _classifiedContractRepository.GetClassifiedContractsCountAsync();
        }
    }
}
