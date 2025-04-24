using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.MasterEscalationMatrixContracts.Command
{
    public class UpdateEscalationMatrixContractHandler : IRequestHandler<UpdateEscalationMatrixContractCommand, int>
    {
        private readonly IMasterEscalationMatrixContractRepository _repository;

        public UpdateEscalationMatrixContractHandler(IMasterEscalationMatrixContractRepository repository)
        {
            _repository = repository;
        }
        public Task<int> Handle(UpdateEscalationMatrixContractCommand request, CancellationToken cancellationToken)
        {
            return _repository.UpdateMatrixContract(request.id,request.updateDto);
        }
    }
}
