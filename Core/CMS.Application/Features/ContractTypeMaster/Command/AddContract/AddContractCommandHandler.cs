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

        private readonly ICacheService _cacheService;
        public AddContractCommandHandler(IContractTypeMasterRepository contractTypeMasterRepository, IMapper Imapper, ICacheService cacheService)
        {
            _contractTypeMasterRepository = contractTypeMasterRepository;
            _Imapper = Imapper;
            _cacheService = cacheService;
        }
        public async Task<ContractTypeMasters> Handle(AddContractCommand request, CancellationToken cancellationToken)
        {
            var mapcontract =  _Imapper.Map<ContractTypeMasters>(request.ctp);
            var result = await _contractTypeMasterRepository.AddContractAsync(mapcontract, request.empCode);

            string cacheKey = $"contracts_1_10";

            await _cacheService.RemoveAsync(cacheKey);
            return result;
            // return await _contractTypeMasterRepository.AddContractAsync(mapcontract,request.empCode);
        }
    }
}
