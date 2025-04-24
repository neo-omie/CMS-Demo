using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.ContractTypeMaster.Query.GetAllContract
{
    public class GetAllContractQueryHandler : IRequestHandler<GetAllContractQuery, IEnumerable<ContractTypeMasters>>
    {
        private readonly IContractTypeMasterRepository _contractTypeMasterRepository;

        public GetAllContractQueryHandler(IContractTypeMasterRepository contractTypeMasterRepository)
        {
            _contractTypeMasterRepository = contractTypeMasterRepository;
        }
        public Task<IEnumerable<ContractTypeMasters>> Handle(GetAllContractQuery request, CancellationToken cancellationToken)
        {
            return _contractTypeMasterRepository.GetAllContractAsync(request.pageNumber, request.pageSize);
        }
    }
}
