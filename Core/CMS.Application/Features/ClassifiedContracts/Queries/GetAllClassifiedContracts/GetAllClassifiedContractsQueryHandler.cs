using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.ClassifiedContracts.Queries.GetAllClassifiedContracts
{
    public class GetAllClassifiedContractsQueryHandler : IRequestHandler<GetAllClassifiedContractsQuery, IEnumerable<GetAllClassifiedContractsDto>>
    {
        readonly IClassifiedContractRepository _contractRepository;
        public GetAllClassifiedContractsQueryHandler(IClassifiedContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }
        public async Task<IEnumerable<GetAllClassifiedContractsDto>> Handle(GetAllClassifiedContractsQuery request, CancellationToken cancellationToken)
        {
            return await _contractRepository.GetAllClassifiedContractsAsync(request.pageNumber, request.pageSize);
        }
    }
}
