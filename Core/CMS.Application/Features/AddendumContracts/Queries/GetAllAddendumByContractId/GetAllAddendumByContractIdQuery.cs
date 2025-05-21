using MediatR;

namespace CMS.Application.Features.AddendumContracts.Queries.GetAllAddendumByContractId
{
    public record GetAllAddendumByContractIdQuery(int pageNumber, int pageSize, int id) :IRequest<object>;
}
