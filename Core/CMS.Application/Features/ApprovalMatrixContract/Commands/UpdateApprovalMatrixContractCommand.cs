using MediatR;

namespace CMS.Application.Features.ApprovalMatrixContract.Commands
{
    public record UpdateApprovalMatrixContractCommand(int id, UpdateApprovalMatrixContractDto contract) : IRequest<bool>;
}
