using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities.CompanyMaster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterCompanyCascade.Queries.GetCityById
{
    public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, ListofCity>
    {
        readonly ICompanyCascadeRepository _companyCascadeRepository;
        public GetCityByIdQueryHandler(ICompanyCascadeRepository companyCascadeRepository)
        {
            _companyCascadeRepository = companyCascadeRepository;
        }
        public async Task<ListofCity> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
        {
            return await _companyCascadeRepository.GetCityById(request.id);
        }
    }
}
