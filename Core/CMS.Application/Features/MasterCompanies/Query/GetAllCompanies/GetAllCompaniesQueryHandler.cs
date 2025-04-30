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
       
        public GetAllCompaniesQueryHandler(IMasterCompanyRepository comprepo)
        {
            _comprepo = comprepo;
           
        }
        public Task<IEnumerable<GetMastersDTO>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
        {
            return _comprepo.GetAllCompanyDetailsAsync(request?.searchTerm, request.pageNumber, request.pageSize);
        }
    }
}
