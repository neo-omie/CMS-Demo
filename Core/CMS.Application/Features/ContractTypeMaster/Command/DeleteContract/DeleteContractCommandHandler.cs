using CMS.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.ContractTypeMaster.Command.DeleteContract
{
    public class DeleteContractCommandHandler : IRequestHandler<DeleteContractCommand, bool>
    {
        private readonly IContractTypeMasterRepository _contractTypeMasterRepository;
        public DeleteContractCommandHandler(IContractTypeMasterRepository contractTypeMasterRepository)
        {
            _contractTypeMasterRepository = contractTypeMasterRepository;
        }
        public Task<bool> Handle(DeleteContractCommand request, CancellationToken cancellationToken)
        {
            return _contractTypeMasterRepository.DeletContract(request.id);
        }
    }
}
