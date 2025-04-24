using CMS.Application.Contracts.Persistence;
using CMS.Application.Exceptions;
using CMS.Application.Features.ApprovalMatrixContract.Queries.GetAllApprovalMatrixContract;
using CMS.Application.Features.ApprovalMatrixContract.Queries.GetApprovalMatrixContractById;
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

        public async Task<GetApprovalMatrixContractByIdDto> GetApprovalMatrixContractById(int id)
        {
            MasterApprovalMatrixContract contractIfo = await _context.MasterApprovalMatrixContracts.FirstOrDefaultAsync(c => c.MasterApprovalMatrixContractId == id);

            if (contractIfo == null)
            {
                throw new NotFoundException($"Approval contract with value id: {id} not found");
            }
            return await _context.MasterApprovalMatrixContracts.Where(c => c.MasterApprovalMatrixContractId == id)
                .Select(c => new GetApprovalMatrixContractByIdDto
                {
                    MasterApprovalMatrixContractId = c.MasterApprovalMatrixContractId,
                    DepartmentId = c.DepartmentId,
                    DepartmentName = c.Department.DepartmentName,
                    ApproverName1 = c.Approver1.EmployeeName,
                    ApproverName2 = c.Approver2.EmployeeName,
                    ApproverName3 = c.Approver3.EmployeeName,
                    NumberOfDays = c.NumberOfDays
                }).FirstOrDefaultAsync();
        }
    }
}
