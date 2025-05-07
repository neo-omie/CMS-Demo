using CMS.Application.Features.MasterApostilles.ApostilleDtos;
using MediatR;

namespace CMS.Application.Features.MasterApostilles.Queries.GetAllApostille
{
    public record GetAllApostilleQuery(int pageNumber, int pageSize, string? searchTerm) :IRequest<object>;
}
