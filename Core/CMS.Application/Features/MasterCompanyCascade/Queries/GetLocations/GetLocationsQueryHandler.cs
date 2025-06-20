using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities.CompanyMaster;
using MediatR;

namespace CMS.Application.Features.MasterCompanyCascade.Queries.GetLocations
{
    public class GetLocationsQueryHandler : IRequestHandler<GetLocationsQuery, IEnumerable<ListOfLocation>>
    {
        private readonly ICompanyCascadeRepository companyCascadeRepository;

        public GetLocationsQueryHandler(ICompanyCascadeRepository _companyCascadeRepository)
        {
            companyCascadeRepository = _companyCascadeRepository;
        }
        public async Task<IEnumerable<ListOfLocation>> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
        {
            var locations = await companyCascadeRepository.GetLocations(request.cityId);
            return locations;
        }
    }
}
