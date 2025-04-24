using AutoMapper;
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
        private readonly IMapper _Imapper;
        public AddContractCommandHandler(IContractTypeMasterRepository contractTypeMasterRepository, IMapper Imapper)
        {
            _contractTypeMasterRepository = contractTypeMasterRepository;
            _Imapper = Imapper;
        }
        public async Task<ContractTypeMasters> Handle(AddContractCommand request, CancellationToken cancellationToken)
        {
            var mapcontract =  _Imapper.Map<ContractTypeMasters>(request.ctp);
            return await _contractTypeMasterRepository.AddContractAsync(mapcontract);
        }
    }
}
