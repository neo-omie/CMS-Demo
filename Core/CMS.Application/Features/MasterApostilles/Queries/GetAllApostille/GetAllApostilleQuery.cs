using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterApostilles.Queries.GetAllApostille
{
    public record GetAllApostilleQuery(string searchTerm, int pageNumber, int pageSize) :IRequest<IEnumerable<MasterApostille>>;
}
