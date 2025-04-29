using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Contracts.Queries.GetContractById
{
    public class GetContractByIdQueryHandler : IRequestHandler<GetContractByIdQuery, GetContractByIdDto>
    {
        readonly IContractRepository _contractRepository;
        public GetContractByIdQueryHandler(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }
        public async Task<GetContractByIdDto> Handle(GetContractByIdQuery request, CancellationToken cancellationToken)
        {
            return await _contractRepository.GetContractByIdAsync(request.id);
        }
    }
}
