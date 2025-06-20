using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities.CompanyMaster;
using MediatR;

namespace CMS.Application.Features.MasterCompanyCascade.Queries.GetLocationById
{
    public class GetLocationByIdQueryHandler : IRequestHandler<GetLocationByIdQuery, ListOfLocation>
    {
        private readonly ICompanyCascadeRepository _companyCascadeRepository;
        public GetLocationByIdQueryHandler(ICompanyCascadeRepository companyCascadeRepository)
        {
            _companyCascadeRepository = companyCascadeRepository;
        }

        public async Task<ListOfLocation> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
        {
            var location = await _companyCascadeRepository.GetLocationById(request.cityId);
            return location;
        }
    }
}
