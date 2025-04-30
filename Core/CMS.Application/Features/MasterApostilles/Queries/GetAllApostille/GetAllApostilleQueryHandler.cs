using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.MasterApostilles.ApostilleDtos;
using MediatR;

namespace CMS.Application.Features.MasterApostilles.Queries.GetAllApostille
{
    public class GetAllApostilleQueryHandler : IRequestHandler<GetAllApostilleQuery, IEnumerable<GetAllApostilleDto>>
    {
        private readonly IMasterApostilleRepository _masterApostilleRepository;
        public GetAllApostilleQueryHandler(IMasterApostilleRepository masterApostilleRepository)
        {
            _masterApostilleRepository = masterApostilleRepository;
        }
        public async Task<IEnumerable<GetAllApostilleDto>> Handle(GetAllApostilleQuery request, CancellationToken cancellationToken)
        {
            return await _masterApostilleRepository.GetAllMasterApostilleAsync(request.pageNumber, request.pageSize);
        }
    }
}
