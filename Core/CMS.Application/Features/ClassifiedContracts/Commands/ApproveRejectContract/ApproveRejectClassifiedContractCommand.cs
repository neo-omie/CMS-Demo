using CMS.Domain.Constants;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.ClassifiedContracts.Commands.ApproveRejectContract
{
    public record ApproveRejectClassifiedContractCommand(int id, string empCode, ContractStatus status) : IRequest<ClassifiedContract>;
}
