using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Exceptions;
using CMS.Application.Features.ApprovalMatrixMOU.Commands.UpdateApprovalMatrixMOU;
using CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOU;
using CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOUById;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CMS.Persistence.Repositories
{
    public class MasterApprovalMatrixMOURepository : IMasterApprovalMatrixMOURepository
    {
        readonly CMSDbContext _context;
        public MasterApprovalMatrixMOURepository(CMSDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<GetAllApprovalMatrixMOUDto>> GetAllApprovalMatrixMOU(int pageNumber, int pageSize)
        {
            int totalRecords = await _context.MasterApprovalMatrixMOUs.CountAsync();
            return _context.MasterApprovalMatrixMOUs.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Select(a => new GetAllApprovalMatrixMOUDto
                {
                    MasterApprovalMatrixMOUId = a.MasterApprovalMatrixMOUId,
                    DepartmentName = a.Department.DepartmentName,
                    ApproverName1 = a.Approver1.EmployeeName, 
                    ApproverName2 = a.Approver2.EmployeeName,
                    ApproverName3 = a.Approver3.EmployeeName,
                    TotalRecords = totalRecords
                });
        }

        public async Task<GetAllApprovalMatrixMOUByIdDto> GetApprovalMatrixMOUById(int id)
        {
            MasterApprovalMatrixMOU mouInfo = await _context.MasterApprovalMatrixMOUs.FirstOrDefaultAsync(c => c.MasterApprovalMatrixMOUId == id);

            if (mouInfo == null)
            {
                throw new NotFoundException($"Approval contract with value id: {id} not found");
            }
            return await _context.MasterApprovalMatrixMOUs.Where(c => c.MasterApprovalMatrixMOUId == id)
                .Select(c => new GetAllApprovalMatrixMOUByIdDto
                {
                    MasterApprovalMatrixMOUId = c.MasterApprovalMatrixMOUId,
                    DepartmentName = c.Department.DepartmentName,
                    ApproverName1 = c.Approver1.EmployeeName,
                    ApproverName2 = c.Approver2.EmployeeName,
                    ApproverName3 = c.Approver3.EmployeeName,
                    NumberOfDays = c.NumberOfDays
                }).FirstOrDefaultAsync();
        }

        public async Task<MasterApprovalMatrixMOU> UpdateApprovalMatrixMOU(int id, UpdateApprovalMatrixMOUDto mou)
        {
            var checkApprMatrMOU = await _context.MasterApprovalMatrixMOUs.FirstOrDefaultAsync(m => m.MasterApprovalMatrixMOUId == id);
            if(checkApprMatrMOU == null)
            {
                throw new NotFoundException($"Approval Matrix MOU with value ID {id} not found.");
            }
            checkApprMatrMOU.ApproverId1 = mou.ApproverId1;
            checkApprMatrMOU.ApproverId2 = mou.ApproverId2;
            checkApprMatrMOU.ApproverId3 = mou.ApproverId3;
            checkApprMatrMOU.NumberOfDays = mou.NumberOfDays;
            _context.MasterApprovalMatrixMOUs.Update(checkApprMatrMOU);
            if(await _context.SaveChangesAsync() > 0)
            {
                return checkApprMatrMOU;
            }
            else
            {
                throw new Exception("For some reasons, data has not been updated :/");
            }
        }
    }
}
