using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.EscalationMatrixMouMaster.Queries.GetEscalationMatrixMoutById;
using MediatR;

namespace CMS.Application.Features.EscalationMatrixMouMaster.Queries.GetEscalationMatrixMouById
{
    public class GetEscalationMatrixMouByIdQueryHandler : IRequestHandler<GetEscalationMatrixMouByIdQuery, EscalationMatrixMoutDto>
    {
        private readonly IMasterEscalationMatrixMouRepository _mouRepository;

        public GetEscalationMatrixMouByIdQueryHandler(IMasterEscalationMatrixMouRepository mouRepository)
        {
            _mouRepository = mouRepository;

        }
        public Task<EscalationMatrixMoutDto> Handle(GetEscalationMatrixMouByIdQuery request, CancellationToken cancellationToken)
        {
            return _mouRepository.GetEscalationMatrixMou(request.id);
        }
    }
}
