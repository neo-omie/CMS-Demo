using CMS.Application.Features.ApprovalMatrixContract.Queries.GetAllApprovalMatrixContract;
using CMS.Application.Features.ApprovalMatrixContract.Queries.GetApprovalMatrixContractById;
using CMS.Domain.Entities;

namespace CMS.Application.Contracts.Persistence
{
    public interface IMasterApprovalMatrixContractRepository
    {
        public Task<IEnumerable<GetAllApprovalMatrixContractDTO>> GetAllApprovalMatrixContract(int pageNumber, int pageSize);
        public Task<GetApprovalMatrixContractByIdDto> GetApprovalMatrixContractById(int id);
    }
}
