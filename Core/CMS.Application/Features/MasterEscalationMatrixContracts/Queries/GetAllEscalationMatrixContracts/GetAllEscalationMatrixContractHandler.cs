using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.MasterDocuments.Queries.GetAllDocument;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterEscalationMatrixContracts.Queries.GetAllEscalationMatrixContracts
{
    public class GetAllEscalationMatrixContractHandler : IRequestHandler<GetAllEscalationMatrixContractQuery, (IEnumerable<GetEscalationMatrixContractDto>, int)>
    {
        private readonly IMasterEscalationMatrixContractRepository _contractRepository;
        private readonly ICacheService _cacheService;

        public GetAllEscalationMatrixContractHandler(IMasterEscalationMatrixContractRepository contractRepository, ICacheService cacheService)
        {
           _contractRepository = contractRepository;
            _cacheService = cacheService;

        }
        public async Task<(IEnumerable<GetEscalationMatrixContractDto>, int)> Handle(GetAllEscalationMatrixContractQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"EscMatrCont_{request.pageNumber}_{request.pageSize}";

            //getting from cache
            var cachedExcCont = await _cacheService.GetAsync<(IEnumerable<GetEscalationMatrixContractDto> MaMc, int totalCount)>(cacheKey);
            if (cachedExcCont.MaMc!=null && cachedExcCont.totalCount != null)
            {
                return (cachedExcCont.MaMc, cachedExcCont.totalCount);
            }
            //not in cache then fetching from repo
            var contrrr = await _contractRepository.GetAllEscalationMatrixContract(request.pageNumber, request.pageSize);

            //storing in cache
            await _cacheService.SetAsync(cacheKey, contrrr, TimeSpan.FromMinutes(1));

            return (contrrr.contr, contrrr.totcount);
            //return _contractRepository.GetAllEscalationMatrixContract(request.pageNumber,request.pageSize);
        }
    }
}
