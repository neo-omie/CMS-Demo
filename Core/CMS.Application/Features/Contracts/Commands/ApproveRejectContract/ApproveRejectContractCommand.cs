using CMS.Domain.Constants;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Contracts.Commands.ApproveRejectContract
{
    public record ApproveRejectContractCommand(int id, string empCode, ContractStatus status) : IRequest<Contract>;
}
