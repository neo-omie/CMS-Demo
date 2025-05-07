using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities.CompanyMaster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterCompanyCascade.Queries.GetCountryById
{
    public class GetCountryByIdQueryHandler : IRequestHandler<GetCountryByIdQuery, ListOfCountries>
    {
        readonly ICompanyCascadeRepository _companyCascadeRepository;
        public GetCountryByIdQueryHandler(ICompanyCascadeRepository companyCascadeRepository)
        {
            _companyCascadeRepository = companyCascadeRepository;
        }
        public async Task<ListOfCountries> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
        {
            return await _companyCascadeRepository.GetCountryById(request.id);
        }
    }
}
