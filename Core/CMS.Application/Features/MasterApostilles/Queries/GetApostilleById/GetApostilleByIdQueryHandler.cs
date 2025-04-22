using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.MasterApostilles.Queries.GetApostilleById
{
    public class GetApostilleByIdQueryHandler:IRequestHandler<GetApostilleByIdQuery, CMS.Domain.Entities.MasterApostille>
    {
        private readonly IMasterApostilleRepository _masterApostilleRepository;
        public GetApostilleByIdQueryHandler(IMasterApostilleRepository masterApostilleRepository)
        {
            _masterApostilleRepository = masterApostilleRepository;
        }
        public async Task<CMS.Domain.Entities.MasterApostille> Handle(GetApostilleByIdQuery request, CancellationToken cancellationToken)
        {
            return await _masterApostilleRepository.GetMasterApostilleByIdAsync(request.Id);
        }
    }
}
