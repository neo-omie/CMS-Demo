using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.MasterApostilles.ApostilleDtos;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterApostilles.Queries.GetAllApostille
{
    public class GetAllApostilleQueryHandler : IRequestHandler<GetAllApostilleQuery, object>
    {
        private readonly IMasterApostilleRepository _masterApostilleRepository;
        private readonly ICacheService _cacheService;
        public GetAllApostilleQueryHandler(IMasterApostilleRepository masterApostilleRepository, ICacheService cacheService)
        {
            _masterApostilleRepository = masterApostilleRepository;
            _cacheService = cacheService;
        }
        public async Task<object> Handle(GetAllApostilleQuery request, CancellationToken cancellationToken)
        {
            //string cacheKey = $"Apostille_{request.pageNumber}_{request.pageSize}_{request.searchTerm}";

            ////getting from cache
            //var cachedApostille = await _cacheService.GetAsync<(IEnumerable<MasterApostille> Data, int TotalCount)>(cacheKey);
            //if (cachedApostille.Data!=null && cachedApostille.TotalCount !=null)
            //{
            //    return new
            //    {
            //        data = cachedApostille.Data,
            //        totalCount = cachedApostille.TotalCount
            //    };
            //}

            ////not in cache then fecthing from repo
            //var apostillee = await _masterApostilleRepository.GetAllMasterApostilleAsync(request.pageNumber, request.pageSize, request.searchTerm);

            ////storing in cache
            //await _cacheService.SetAsync(cacheKey, apostillee, TimeSpan.FromMinutes(1));

           var result = await _masterApostilleRepository.GetAllMasterApostilleAsync(request.pageNumber, request.pageSize, request?.searchTerm);
           
            return new
            {
                data = result.Data,
                totalCount = result.TotalCount
            }; 
        }
    }
}
