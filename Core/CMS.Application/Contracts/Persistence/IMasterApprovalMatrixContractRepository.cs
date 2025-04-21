using CMS.Application.Features.ApprovalMatrixContract.Queries.GetAllApprovalMatrixContract;
using CMS.Domain.Entities;

namespace CMS.Application.Contracts.Persistence
{
    public interface IMasterApprovalMatrixContractRepository
    {
        public Task<IEnumerable<GetAllApprovalMatrixContractDTO>> GetAllApprovalMatrixContract(int pageNumber, int pageSize);
    }
}
