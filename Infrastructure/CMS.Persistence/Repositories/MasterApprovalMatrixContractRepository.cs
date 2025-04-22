using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.ApprovalMatrixContract.Queries.GetAllApprovalMatrixContract;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using MediatR;
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
            int totalRecords = await _context.MasterApprovalMatrixContracts.CountAsync();
            return _context.MasterApprovalMatrixContracts.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Select(a => new GetAllApprovalMatrixContractDTO
                {
                    MasterApprovalMatrixContractId = a.MasterApprovalMatrixContractId,
                    DepartmentName = a.Department.DepartmentName,
                    ApproverName1 = a.Approver1.EmployeeName,
                    ApproverName2 = a.Approver2.EmployeeName,
                    ApproverName3 = a.Approver3.EmployeeName,
                    TotalRecords = totalRecords
                });
        }

        public async Task<IEnumerable<GetAllApprovalMatrixContractDTO>> GetApprovalMatrixContractById(int pageNumber, int pageSize)
        {
            return _context.MasterApprovalMatrixContracts.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Select(a => new GetAllApprovalMatrixContractDTO
                {
                    MasterApprovalMatrixContractId = a.MasterApprovalMatrixContractId,
                    DepartmentName = a.Department.DepartmentName,
                    ApproverName1 = a.Approver1.EmployeeName,
                    ApproverName2 = a.Approver2.EmployeeName,
                    ApproverName3 = a.Approver3.EmployeeName
                });
        }
    }
}
