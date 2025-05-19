using CMS.Application.Features.Contracts.Queries.GetContractById;
using MediatR;

namespace CMS.Application.Features.Contracts.Queries.GetContractByContractName
{
    public record GetContractByContractNameQuery(string name):IRequest<GetContractByIdDto>;
}
