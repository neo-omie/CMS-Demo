using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.Contracts.Queries.GetContractById;
using MediatR;

namespace CMS.Application.Features.Contracts.Queries.GetContractByContractName
{
    public class GetContractByContractNameQueryHandler : IRequestHandler<GetContractByContractNameQuery, GetContractByIdDto>
    {
        readonly IContractRepository _contractRepository;
        public GetContractByContractNameQueryHandler(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }
        public Task<GetContractByIdDto> Handle(GetContractByContractNameQuery request, CancellationToken cancellationToken)
        {
            return _contractRepository.GetContractByNameAsync(request.name);
        }
    }
}
