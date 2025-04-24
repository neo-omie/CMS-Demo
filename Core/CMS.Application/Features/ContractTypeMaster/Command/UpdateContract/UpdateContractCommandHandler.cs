using AutoMapper;
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
        private readonly IMapper _Imapper;

        public UpdateContractCommandHandler(IContractTypeMasterRepository contractTypeMasterRepository, IMapper Imapper)
        {
            _contractTypeMasterRepository = contractTypeMasterRepository;
            _Imapper = Imapper;
        }
        public async Task<ContractTypeMasters> Handle(UpdateContractCommand request, CancellationToken cancellationToken)
        {
            var cont = _contractTypeMasterRepository.GetContractById(request.id);
            if (cont==null)
            {
                throw new Exception($"Contract not found");
            }
            var mapcontract = _Imapper.Map<ContractTypeMasters>(request.ctp);
            return await _contractTypeMasterRepository.UpdateContractAsync(request.id, mapcontract);
        }
    }
}
