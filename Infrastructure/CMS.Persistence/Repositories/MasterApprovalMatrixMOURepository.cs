using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Exceptions;
using CMS.Application.Features.ApprovalMatrixContract.Queries.GetApprovalMatrixContractById;
using CMS.Application.Features.ApprovalMatrixMOU.Commands.UpdateApprovalMatrixMOU;
using CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOU;
using CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOUById;
using CMS.Domain.Constants;
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
            string query = "EXEC SP_GetAllApprovalMatrixMOUDto @pageNumber = {0}, @pageSize = {1}";
            return await  _context.GetAllApprovalMatrixMOUDtos.FromSqlRaw(query, pageNumber, pageSize).ToListAsync();
        }

        public async Task<GetAllApprovalMatrixMOUByIdDto> GetApprovalMatrixMOUById(int id)
        {
            string query = "EXEC SP_GetApprovalMatrixMOUById @id = {0}";
            IEnumerable<GetAllApprovalMatrixMOUByIdDto> result = _context.GetAllApprovalMatrixMOUByIdDtos.FromSqlRaw(query, id);
            return result.FirstOrDefault();
        }

        public async Task<bool> UpdateApprovalMatrixMOU(int id, UpdateApprovalMatrixMOUDto mou,string empCode)
        {
            var approval = await _context.MasterApprovalMatrixMOUs.FindAsync(id);

            string query = "EXEC SP_UpdateApprovalMatrixMOU @id = {0}, @approverId1 = {1}, @approverId2 = {2}, @approverId3 = {3}, @numberOfDays = {4}";
            await _context.Database.ExecuteSqlRawAsync(query, id, mou.ApproverId1, mou.ApproverId2, mou.ApproverId3, mou.NumberOfDays);
            if (await _context.SaveChangesAsync() > 0)
            {

                string auditQuery = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
            await _context.Database.ExecuteSqlRawAsync(query, id, TableList.MasterApprovalMatrixMOU, $"Approval for  {approval.Department.DepartmentName} has been Updated by {empCode} ", empCode, LogStatus.Updated);

                return true;
            }
            return false;


        }
    }
}
