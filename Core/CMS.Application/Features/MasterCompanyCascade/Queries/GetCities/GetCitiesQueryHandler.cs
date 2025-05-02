using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities.CompanyMaster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterCompanyCascade.Queries.GetCities
{
    public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, IEnumerable<ListofCity>>
    {
        readonly ICompanyCascadeRepository _companyCascadeRepository;

        public GetCitiesQueryHandler(ICompanyCascadeRepository companyCascadeRepository)
        {
            _companyCascadeRepository = companyCascadeRepository;
        }
        public async Task<IEnumerable<ListofCity>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
        {
            var check = await _companyCascadeRepository.GetCities(request.id);
            return check;
        }
    }
}
