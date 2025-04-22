using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.ContractTypeMaster.Command.UpdateContract
{
    public class UpdateContractCommandHandler : IRequestHandler<UpdateContractCommand, ContractTypeMasters>
    {
        private readonly IContractTypeMasterRepository _contractTypeMasterRepository;

        public UpdateContractCommandHandler(IContractTypeMasterRepository contractTypeMasterRepository)
        {
            _contractTypeMasterRepository = contractTypeMasterRepository;
        }
        public Task<ContractTypeMasters> Handle(UpdateContractCommand request, CancellationToken cancellationToken)
        {
            var cont = _contractTypeMasterRepository.GetContractById(request.id);
            if (cont==null)
            {
                throw new Exception($"Contract not found");
            }
            return _contractTypeMasterRepository.UpdateContractAsync(request.id, request.ctp);
        }
    }
}
