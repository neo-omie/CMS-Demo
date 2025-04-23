using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities.CompanyMaster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterCompanyCascade.Queries.GetCountries
{
    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, IEnumerable<ListOfCountries>>
    {
        readonly ICompanyCascadeRepository _companyCascadeRepository;

        public GetCountriesQueryHandler( ICompanyCascadeRepository companyCascadeRepository)
        {
            _companyCascadeRepository = companyCascadeRepository;
        }
        public async Task<IEnumerable<ListOfCountries>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            return await _companyCascadeRepository.GetCountries();
        }
    }
}
