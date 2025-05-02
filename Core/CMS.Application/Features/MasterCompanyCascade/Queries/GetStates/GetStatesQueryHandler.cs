using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities.CompanyMaster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterCompanyCascade.Queries.GetStates
{
    public class GetStatesQueryHandler : IRequestHandler<GetStatesQuery,IEnumerable<ListOfStates>>
    {
        readonly ICompanyCascadeRepository _companyCascadeRepository;

        public GetStatesQueryHandler( ICompanyCascadeRepository companyCascadeRepository)
        {
            _companyCascadeRepository = companyCascadeRepository;
        }
        public async Task<IEnumerable<ListOfStates>> Handle(GetStatesQuery request, CancellationToken cancellationToken)
        {
            var check = await _companyCascadeRepository.GetStates(request.id);
            return check;
        }
    }
}
