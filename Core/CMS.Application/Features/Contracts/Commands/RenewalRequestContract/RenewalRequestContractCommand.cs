using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Contracts.Commands.RenewalRequestContract
{
    public record RenewalRequestContractCommand(int id, string empCode) : IRequest<Contract>;
}
