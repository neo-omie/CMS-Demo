using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.ContractTypeMaster.Query.GetAllContract
{
    public class GetAllContractQueryHandler : IRequestHandler<GetAllContractQuery, IEnumerable<GetAllContractTypesDTO>>
    {
        private readonly ICacheService _cacheService;

        private readonly IContractTypeMasterRepository _contractTypeMasterRepository;

        public GetAllContractQueryHandler(IContractTypeMasterRepository contractTypeMasterRepository, ICacheService cacheService)
        {
            _contractTypeMasterRepository = contractTypeMasterRepository;
            _cacheService = cacheService;
        }
        public async Task<IEnumerable<GetAllContractTypesDTO>> Handle(GetAllContractQuery request, CancellationToken cancellationToken)
        {
            //string cacheKey = $"contracts_{request.pageNumber}_{request.pageSize}";

            ////getting from cache
            //var cachedContracts = await _cacheService.GetAsync<IEnumerable<GetAllContractTypesDTO>>(cacheKey);
            //if (cachedContracts!=null)
            //{
            //    return cachedContracts;
            //}

            ////not in cache then fetching from repo
            //var contracts = await _contractTypeMasterRepository.GetAllContractAsync(
            //    request.pageNumber, request.pageSize
            //    );

            ////store in cache
            //await _cacheService.SetAsync(cacheKey, contracts, TimeSpan.FromMinutes(1));

            //return contracts;


            return await  _contractTypeMasterRepository.GetAllContractAsync(request.pageNumber, request.pageSize);
        }
    }
}
