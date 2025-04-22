using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.MasterApostilles.Queries.GetAllApostille
{
    public class GetAllApostilleQueryHandler : IRequestHandler<GetAllApostilleQuery, IEnumerable<CMS.Domain.Entities.MasterApostille>>
    {
        private readonly IMasterApostilleRepository _masterApostilleRepository;
        public GetAllApostilleQueryHandler(IMasterApostilleRepository masterApostilleRepository)
        {
            _masterApostilleRepository = masterApostilleRepository;
        }
        public async Task<IEnumerable<CMS.Domain.Entities.MasterApostille>> Handle(GetAllApostilleQuery request, CancellationToken cancellationToken)
        {
            return await _masterApostilleRepository.GetAllMasterApostilleAsync(request.searchTerm, request.pageNumber, request.pageSize);
        }
    }
}
