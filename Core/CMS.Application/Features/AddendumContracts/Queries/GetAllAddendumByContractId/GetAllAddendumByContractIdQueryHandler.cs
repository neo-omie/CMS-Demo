using CMS.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.AddendumContracts.Queries.GetAllAddendumByContractId
{
    public class GetAllAddendumByContractIdQueryHandler : IRequestHandler<GetAllAddendumByContractIdQuery, object>
    {
        private readonly IAddendumContractRepository _addendumContractRepository;

        public GetAllAddendumByContractIdQueryHandler(IAddendumContractRepository addendumContractRepository)
        {
            _addendumContractRepository = addendumContractRepository;
        }
        public async Task<object> Handle(GetAllAddendumByContractIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _addendumContractRepository.GetAllAddendumByContractIdAsync(request.pageNumber, request.pageSize, request.id);
            return new
            {
                data = result.Data,
                totalCount = result.TotalCount,
            };
        }
    }
}
