using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.ApprovalMatrixContract.Commands;
using CMS.Application.Features.ApprovalMatrixContract.Queries.GetAllApprovalMatrixContract;
using CMS.Application.Features.ApprovalMatrixContract.Queries.GetApprovalMatrixContractById;
using CMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CMS.Persistence.Repositories
{
    public class MasterApprovalMatrixContractRepository : IMasterApprovalMatrixContractRepository
    {
        readonly CMSDbContext _context;
        public MasterApprovalMatrixContractRepository(CMSDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<GetAllApprovalMatrixContractDTO>> GetAllApprovalMatrixContract(int pageNumber, int pageSize)
        {
            string query = "EXEC SP_GetAllApprovalMatrixContract @pageNumber = {0}, @pageSize = {1}";
            return _context.GetAllApprovalMatrixContractDTOs.FromSqlRaw(query, pageNumber, pageSize);
        }
        public async Task<GetApprovalMatrixContractByIdDto> GetApprovalMatrixContractById(int id)
        {
            string query = "EXEC SP_GetApprovalMatrixContractById @id = {0}";
            IEnumerable<GetApprovalMatrixContractByIdDto> result = _context.GetApprovalMatrixContractByIdDtos.FromSqlRaw(query, id);
            return result.FirstOrDefault();
        }
        public async Task<bool> UpdateApprovalMatrixContract(int id, UpdateApprovalMatrixContractDto contract)
        {
            string query = "EXEC SP_UpdateApprovalMatrixContract @id = {0}, @ApproverId1 = {1}, @ApproverId2 = {2}, @ApproverId3 = {3}, @NumberOfDays = {4}";
            return await _context.Database.ExecuteSqlRawAsync(query, id, contract.ApproverId1, contract.ApproverId2, contract.ApproverId3, contract.NumberOfDays ) > 0;
        }
    }
}
