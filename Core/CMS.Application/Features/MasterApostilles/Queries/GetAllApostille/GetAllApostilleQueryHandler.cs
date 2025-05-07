using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.MasterApostilles.ApostilleDtos;
using MediatR;

namespace CMS.Application.Features.MasterApostilles.Queries.GetAllApostille
{
    public class GetAllApostilleQueryHandler : IRequestHandler<GetAllApostilleQuery, object>
    {
        private readonly IMasterApostilleRepository _masterApostilleRepository;
        public GetAllApostilleQueryHandler(IMasterApostilleRepository masterApostilleRepository)
        {
            _masterApostilleRepository = masterApostilleRepository;
        }
        public async Task<object> Handle(GetAllApostilleQuery request, CancellationToken cancellationToken)
        {
            var result = await _masterApostilleRepository.GetAllMasterApostilleAsync(request.pageNumber, request.pageSize, request?.searchTerm);
            return new
            {
                data = result.Data,
                totalCount = result.TotalCount
            }; 
        }
    }
}
