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
        private readonly ICacheService _cacheService;

        public UpdateContractCommandHandler(IContractTypeMasterRepository contractTypeMasterRepository, IMapper Imapper, ICacheService cacheService)
        {
            _contractTypeMasterRepository = contractTypeMasterRepository;
            _Imapper = Imapper;
            _cacheService = cacheService;
        }
        public async Task<ContractTypeMasters> Handle(UpdateContractCommand request, CancellationToken cancellationToken)
        {
            var cont = await _contractTypeMasterRepository.GetContractById(request.id);
            if (cont==null)
            {
                throw new Exception($"Contract not found");
            }
            var mapcontract = _Imapper.Map<ContractTypeMasters>(request.ctp);
            var updated = await  _contractTypeMasterRepository.UpdateContractAsync(request.id, mapcontract,request.empCode);

            //clearing cache for fresh data
            string cacheKey = $"contracts_1_10";
            await _cacheService.RemoveAsync(cacheKey);

            return updated;
        }
    }
}
