using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities.CompanyMaster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterCompanyCascade.Queries.GetStateById
{
    public class GetStateByIdQueryHandler : IRequestHandler<GetStateByIdQuery, ListOfStates>
    {
        readonly ICompanyCascadeRepository _companyCascadeRepository;
        public GetStateByIdQueryHandler(ICompanyCascadeRepository companyCascadeRepository)
        {
            _companyCascadeRepository = companyCascadeRepository;
        }
        public async Task<ListOfStates> Handle(GetStateByIdQuery request, CancellationToken cancellationToken)
        {
            return await _companyCascadeRepository.GetStateById(request.id);
        }
    }
}
