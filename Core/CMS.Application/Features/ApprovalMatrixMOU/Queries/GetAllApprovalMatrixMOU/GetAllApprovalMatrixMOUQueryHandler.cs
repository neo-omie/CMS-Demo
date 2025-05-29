using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOU
{
    public class GetAllApprovalMatrixMOUQueryHandler : IRequestHandler<GetAllApprovalMatrixMOUQuery, IEnumerable<GetAllApprovalMatrixMOUDto>>
    {
        readonly IMasterApprovalMatrixMOURepository _masterApprovalMatrixMOURepository;
        private readonly ICacheService _cacheService;
        public GetAllApprovalMatrixMOUQueryHandler(IMasterApprovalMatrixMOURepository masterApprovalMatrixMOURepository, ICacheService cacheService)
        {
            _masterApprovalMatrixMOURepository = masterApprovalMatrixMOURepository;
            _cacheService = cacheService;
        }
        public async Task<IEnumerable<GetAllApprovalMatrixMOUDto>> Handle(GetAllApprovalMatrixMOUQuery request, CancellationToken cancellationToken)
        {
            //string cacheKey = $"MAMO_{request.pageNumber}_{request.pageSize}";

            ////getting from cache 
            //var cachedMomu = await _cacheService.GetAsync<IEnumerable<GetAllApprovalMatrixMOUDto>>(cacheKey);
            //if (cachedMomu !=null)
            //{
            //    return cachedMomu;
            //}

            ////if not in cache then fetching from repo 
            //var Cmomu = await _masterApprovalMatrixMOURepository.GetAllApprovalMatrixMOU(request.pageNumber, request.pageSize);

            ////store in cache 

            //await _cacheService.SetAsync(cacheKey, Cmomu, TimeSpan.FromMinutes(1));

            //return Cmomu;
            return await _masterApprovalMatrixMOURepository.GetAllApprovalMatrixMOU(request.pageNumber, request.pageSize);
        }
    }
}
