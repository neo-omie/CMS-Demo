using System.Net.Mail;
using System.Net;
using AutoMapper;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.ApprovalMatrixContract.Queries.GetAllApprovalMatrixContract
{
    public class GetAllApprovalMatrixContractQueryHandler : IRequestHandler<GetAllApprovalMatrixContractQuery, IEnumerable<GetAllApprovalMatrixContractDTO>>
    {
        private readonly IMasterApprovalMatrixContractRepository _masterApprovalMatrixContractRepository;
        private readonly ICacheService _cacheService;
        public GetAllApprovalMatrixContractQueryHandler(IMasterApprovalMatrixContractRepository masterApprovalMatrixContractRepository, ICacheService cacheService)
        {
            _masterApprovalMatrixContractRepository = masterApprovalMatrixContractRepository;
            _cacheService = cacheService;
        }
        public async Task<IEnumerable<GetAllApprovalMatrixContractDTO>> Handle(GetAllApprovalMatrixContractQuery request, CancellationToken cancellationToken)
        {
            //string cacheKey = $"ApprovalMatrixContract_{request.pageNumber}_{request.pageSize}";

            ////getting from cache 
            //var cachedAmc= await _cacheService.GetAsync<IEnumerable<GetAllApprovalMatrixContractDTO>>(cacheKey);
            //if (cachedAmc != null)
            //{
            //    return cachedAmc;
            //}

            ////not in cache then fetching from repo
            //var Amc = await _masterApprovalMatrixContractRepository.GetAllApprovalMatrixContract(request.pageNumber, request.pageSize
            //    );

            ////store in cache 
            //await _cacheService.SetAsync(cacheKey, Amc, TimeSpan.FromMinutes(1));

            //return Amc;

            return await _masterApprovalMatrixContractRepository.GetAllApprovalMatrixContract(request.pageNumber,request.pageSize);
        }
    }
}
