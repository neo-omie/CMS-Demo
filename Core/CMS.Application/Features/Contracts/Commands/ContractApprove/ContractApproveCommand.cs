using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Contracts.Commands.ContractApproveApprover
{
    public record ContractApproveCommand(int id, string empCode) : IRequest<Contract>;
}
