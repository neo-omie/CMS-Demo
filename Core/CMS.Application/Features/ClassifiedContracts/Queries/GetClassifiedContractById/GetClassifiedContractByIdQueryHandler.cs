using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.ClassifiedContracts.Queries.GetClassifiedContractById
{
    public class GetClassifiedContractByIdQueryHandler : IRequestHandler<GetClassifiedContractByIdQuery, GetClassifiedContractByIdDto>
    {
        readonly IClassifiedContractRepository _contractRepository;
        public GetClassifiedContractByIdQueryHandler(IClassifiedContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }
        public async Task<GetClassifiedContractByIdDto> Handle(GetClassifiedContractByIdQuery request, CancellationToken cancellationToken)
        {
            return await _contractRepository.GetClassifiedContractByIdAsync(request.id);
        }
    }
}
