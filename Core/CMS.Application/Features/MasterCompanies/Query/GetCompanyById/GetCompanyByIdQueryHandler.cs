using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities.CompanyMaster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterCompanies.Query.GetCompanyById
{
  

    
    public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, MasterCompany>
    {
        readonly IMasterCompanyRepository _masterCompanyRepository;
        public GetCompanyByIdQueryHandler(IMasterCompanyRepository masterCompanyRepository)
        {
            _masterCompanyRepository = masterCompanyRepository;
        }
        public async Task<MasterCompany> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            return await _masterCompanyRepository.GetCompanyByIdAsync(request.id);
        }
    }
}
