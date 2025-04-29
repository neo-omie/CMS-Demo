using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.EscalationMatrixMouMaster.Queries.GetAllEscalationMatrixMou
{
    public class GetAllEscalationMatrixMouQueryHandler : IRequestHandler<GetAllEscalationMatrixMouQuery,IEnumerable<EscalationMatrixMoutDto>>
    {
        private readonly IMasterEscalationMatrixMouRepository _mouRepository;

        public GetAllEscalationMatrixMouQueryHandler(IMasterEscalationMatrixMouRepository mouRepository)
        {
            _mouRepository = mouRepository;

        }
        Task<IEnumerable<EscalationMatrixMoutDto>> IRequestHandler<GetAllEscalationMatrixMouQuery, IEnumerable<EscalationMatrixMoutDto>>.Handle(GetAllEscalationMatrixMouQuery request, CancellationToken cancellationToken)
        {
            return _mouRepository.GetAllEscalationMatrixMou(request.pageNumber, request.pageSize);
        }
    }
}
