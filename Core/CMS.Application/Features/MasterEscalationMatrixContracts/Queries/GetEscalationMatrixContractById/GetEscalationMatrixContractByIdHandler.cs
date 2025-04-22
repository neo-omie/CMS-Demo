using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Exceptions;
using MediatR;

namespace CMS.Application.Features.MasterEscalationMatrixContracts.Queries.GetEscalationMatrixContractById
{
    public class GetEscalationMatrixContractByIdHandler : IRequestHandler<GetEscalationMatrixContractByIdQuery, GetEscalationMatrixContractDto>
    {
        private readonly IMasterEscalationMatrixContractRepository _masterEscalationMatrixContractRepository;

        public GetEscalationMatrixContractByIdHandler( IMasterEscalationMatrixContractRepository repository)
        {
            _masterEscalationMatrixContractRepository = repository;
        }
        public Task<GetEscalationMatrixContractDto> Handle(GetEscalationMatrixContractByIdQuery request, CancellationToken cancellationToken)
        {
            var contract =  _masterEscalationMatrixContractRepository.GetEscalationMatrixContract(request.id);
            if(contract == null)
            {
                throw new NotFoundException("Contract not Found!!");
            }

            return contract;
        }
    }

}
