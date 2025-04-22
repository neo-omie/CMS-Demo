using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.ContractTypeMaster.Command.AddContract
{
    public class AddContractCommandHandler : IRequestHandler<AddContractCommand, ContractTypeMasters>
    {
        private readonly IContractTypeMasterRepository _contractTypeMasterRepository;

        public AddContractCommandHandler(IContractTypeMasterRepository contractTypeMasterRepository)
        {
            _contractTypeMasterRepository = contractTypeMasterRepository;
        }
        public Task<ContractTypeMasters> Handle(AddContractCommand request, CancellationToken cancellationToken)
        {
            return _contractTypeMasterRepository.AddContractAsync(request.ctp);
        }
    }
}
