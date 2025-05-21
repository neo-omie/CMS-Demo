using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.AddendumContract.AddendumContractDto;
using CMS.Application.Features.Contracts.Queries.GetAllContracts;
using MediatR;

namespace CMS.Application.Features.AddendumContracts.Queries.GetAllAddendumContracts
{
    public class GetAllAddendumContractsQueryHandler : IRequestHandler<GetAllAddendumContractsQuery, object>
    {
        private readonly IAddendumContractRepository _addendumContractRepository;

        public GetAllAddendumContractsQueryHandler(IAddendumContractRepository addendumContractRepository)
        {
            _addendumContractRepository = addendumContractRepository;
        }
        public async Task<object> Handle(GetAllAddendumContractsQuery request, CancellationToken cancellationToken)
        {
            var result = await _addendumContractRepository.GetAllAddendumContractsAsync(request.pageNumber, request.pageSize, request?.searchTerm);
            return new
            {
                data = result.Data,
                totalCount = result.TotalCount
            };
        }
    }
}
