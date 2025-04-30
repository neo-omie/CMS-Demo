using MediatR;

namespace CMS.Application.Features.EscalationMatrixMouMaster.Queries.GetEscalationMatrixMoutById
{
    public record GetEscalationMatrixMouByIdQuery(int id) : IRequest<EscalationMatrixMoutDto>;
}
