using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.EscalationMatrixMouMaster.Queries.GetAllEscalationMatrixMou
{
    public record GetAllEscalationMatrixMouQuery(int pageNumber, int pageSize) : IRequest<IEnumerable<EscalationMatrixMoutDto>>;


}
