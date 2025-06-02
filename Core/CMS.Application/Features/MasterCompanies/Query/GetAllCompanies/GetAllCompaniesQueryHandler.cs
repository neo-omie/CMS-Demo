using AutoMapper;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities.CompanyMaster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterCompanies.Query.GetAllCompanies
{
    public class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, IEnumerable<GetMastersDTO>>
    {
        private readonly IMasterCompanyRepository _comprepo;
        private readonly ICacheService _cacheService;
       
        public GetAllCompaniesQueryHandler(IMasterCompanyRepository comprepo,ICacheService cacheService )
        {
            _comprepo = comprepo;
            _cacheService = cacheService;

           
        }
        public async Task<IEnumerable<GetMastersDTO>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
        {
            string searchTerm = request.searchTerm ?? "";
            string cacheKey = $"companies_{request.searchTerm}_{request.pageNumber}_{request.pageSize}";

            //getting from cache
            var cachedCompanies = await _cacheService.GetAsync<IEnumerable<GetMastersDTO>>(cacheKey);
            if (cachedCompanies != null)
            {
                return cachedCompanies;
            }

            //not in cache then fetching from repo
            var companies = await _comprepo.GetAllCompanyDetailsAsync(
                request.searchTerm, request.pageNumber, request.pageSize);

            //store in cache
            await _cacheService.SetAsync(cacheKey, companies, TimeSpan.FromMinutes(1));

            return companies;


           // return await _comprepo.GetAllCompanyDetailsAsync(request?.searchTerm, request.pageNumber, request.pageSize);
        }
    }
}
