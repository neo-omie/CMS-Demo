using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.EscalationMatrixMouMaster.Queries.GetAllEscalationMatrixMou
{
    public class GetAllEscalationMatrixMouQueryHandler : IRequestHandler<GetAllEscalationMatrixMouQuery,IEnumerable<EscalationMatrixMoutDto>>
    {
        private readonly IMasterEscalationMatrixMouRepository _mouRepository;
        private readonly ICacheService _cacheService;

        public GetAllEscalationMatrixMouQueryHandler(IMasterEscalationMatrixMouRepository mouRepository, ICacheService cacheService)
        {
            _mouRepository = mouRepository;
            _cacheService = cacheService;

        }
        async Task<IEnumerable<EscalationMatrixMoutDto>> IRequestHandler<GetAllEscalationMatrixMouQuery, IEnumerable<EscalationMatrixMoutDto>>.Handle(GetAllEscalationMatrixMouQuery request, CancellationToken cancellationToken)
        {
            //string cacheKey = $"EscalationMatrixMou_{request.pageNumber}_{request.pageSize}";

            ////getting from cache
            //var cachedEMM = await _cacheService.GetAsync<IEnumerable<EscalationMatrixMoutDto>>(cacheKey);

            //if (cachedEMM != null)
            //{
            //    return cachedEMM;
            //}

            ////not in cache then fetching from repo
            //var Emm = await _mouRepository.GetAllEscalationMatrixMou(request.pageNumber, request.pageSize);

            ////store in cache 
            //await _cacheService.SetAsync(cacheKey, Emm, TimeSpan.FromMinutes(1));

            //return Emm;

            return await _mouRepository.GetAllEscalationMatrixMou(request.pageNumber, request.pageSize);
        }
    }
}
