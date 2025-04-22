using MediatR;

namespace CMS.Application.Features.ApprovalMatrixContract.Queries.GetApprovalMatrixContractById
{
    public record GetApprovalMatrixContractByIdQuery(int id) : IRequest<GetApprovalMatrixContractByIdDto>;
}
