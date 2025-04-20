using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.ApprovalMatrixContract.Queries.GetAllApprovalMatrixContract
{
    public record GetAllApprovalMatrixContractQuery(int pageNumber, int pageSize) : IRequest<IEnumerable<GetAllApprovalMatrixContractDTO>>;
}
