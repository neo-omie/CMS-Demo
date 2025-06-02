using CMS.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterCompanies.Command.DeleteCompany
{
    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, bool>
    {
        private readonly IMasterCompanyRepository _comprepo;
        private readonly ICacheService _cacheService;

        public DeleteCompanyCommandHandler(IMasterCompanyRepository comprepo, ICacheService cacheService)
        {
            _comprepo = comprepo;
            _cacheService = cacheService;
        }
        public async Task<bool> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var result = await  _comprepo.DeleteCompanyAsync(request.id, request.empCode);

            if (result)
            {

                string searchTerm = "";
                int pageSize = 10;
                int maxPagesToClear = 10;

                for (int page = 1;  page< maxPagesToClear; page++)
                {
                    string key = $"companies_{searchTerm}_{page}_{pageSize}";
                    await _cacheService.RemoveAsync(key);
                }
                //string cacheKey = $"companies_1_10";
                //await _cacheService.RemoveAsync(cacheKey);
            }

            return result;

            //return _comprepo.DeleteCompanyAsync(request.id, request.empCode);
        }
    }
}
