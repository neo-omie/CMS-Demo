using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.Departments.Queries.GetDepartmentById;
using CMS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.ContractTypeMaster.Query.GetContractById
{   
    public class GetContractByIdQueryHandler : IRequestHandler<GetContractByIdQuery, ContractTypeMasters>
    {
        readonly IContractTypeMasterRepository _contractTypeMasterRepository;
        public GetContractByIdQueryHandler(IContractTypeMasterRepository contractTypeMasterRepository )
        {
            _contractTypeMasterRepository = contractTypeMasterRepository;
        }
        public async Task<ContractTypeMasters> Handle(GetContractByIdQuery request, CancellationToken cancellationToken)
        {
            return await _contractTypeMasterRepository.GetContractById(request.id);
        }
    }
}
