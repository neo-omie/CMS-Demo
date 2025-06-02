using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities.CompanyMaster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterCompanies.Command.UpdateCompany
{
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, MasterCompany>
    {
        private readonly IMasterCompanyRepository _comprepo;
        private readonly ICacheService _cacheService;

        public UpdateCompanyCommandHandler(IMasterCompanyRepository comprepo, ICacheService cacheService)
        {
            _comprepo = comprepo;
            _cacheService = cacheService;
        }
        public async Task<MasterCompany> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company =await _comprepo.GetCompanyByIdAsync(request.id);
            if (company==null)
            {
                throw new Exception($"Company Not Found");
            }

            var updated = await _comprepo.UpdateCompanyAsync(request.id, request.comp, request.empCode);

            string searchTerm = "";
            int pageSize = 10;
            int maxPagesToClear = 10;

            for (int page = 1; page <= maxPagesToClear; page++)
            {
                string key = $"companies_{searchTerm}_{page}_{pageSize}";
                await _cacheService.RemoveAsync(key);
            }


            return updated;

            //return await _comprepo.UpdateCompanyAsync(request.id, request.comp,request.empCode);
        }
    }
}
