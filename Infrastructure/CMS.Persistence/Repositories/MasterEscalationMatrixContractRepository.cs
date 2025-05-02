using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Exceptions;
using CMS.Application.Features.ApprovalMatrixContract.Queries.GetAllApprovalMatrixContract;

using CMS.Application.Features.MasterEscalationMatrixContracts;
using CMS.Application.Features.MasterEscalationMatrixContracts.Command;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CMS.Persistence.Repositories
{
    public class MasterEscalationMatrixContractRepository : IMasterEscalationMatrixContractRepository
    {
        private readonly CMSDbContext _context;
        public MasterEscalationMatrixContractRepository(CMSDbContext context)
        {
            _context = context;
        }



        public async Task<(IEnumerable<GetEscalationMatrixContractDto>, int)> GetAllEscalationMatrixContract(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("Page size must be greater than 0.");
            }
            var totalCount = await _context.MasterEscalationMatrixContracts.CountAsync();
            string sql = "EXEC SP_GetAllEscalationMatrixContracts @PageNumber = {0}, @PageSize = {1}";
            var allEscalations = _context.GetEscalationMatrixContractDtos.FromSqlRaw(sql,pageNumber, pageSize);
            return (allEscalations, totalCount);
        }

        public async Task<GetEscalationMatrixContractDto> GetEscalationMatrixContract(int valueId)
        {
            var contract =await _context.MasterEscalationMatrixContracts
                .FirstOrDefaultAsync(x=>x.MatrixContractId == valueId);
            if(contract == null)
            {
                throw new NotFoundException($"Escalation matrix contract with id {valueId} not found.");
            }
            return await _context.MasterEscalationMatrixContracts.Where(x => x.MatrixContractId == valueId).Select(x => new GetEscalationMatrixContractDto
            {
                MatrixContractId = x.MatrixContractId,
                DepartmentName = x.Department.DepartmentName,
                DepartmentId = x.DepartmentId,
                Escalation1 = x.Escalation1.EmployeeName,
                Escalation2 = x.Escalation2.EmployeeName,
                Escalation3 = x.Escalation3.EmployeeName,
                EscalationId1 = x.Escalation1.EmployeeCode,
                EscalationId2 = x.Escalation2.EmployeeCode,
                EscalationId3 = x.Escalation3.EmployeeCode,
                TriggerDaysEscalation1 = x.TriggerDaysEscalation1,
                TriggerDaysEscalation2 = x.TriggerDaysEscalation2,
                TriggerDaysEscalation3 = x.TriggerDaysEscalation3

            }).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateMatrixContract(int valueId, UpdateEscalationMatrixContractDto updateDto)
        {
            var contract = await _context.MasterEscalationMatrixContracts.FirstOrDefaultAsync(x => x.MatrixContractId == valueId);
            if (contract == null)
            {
                throw new NotFoundException("Escalation Contract not Found");
            }
            contract.EscalationId1 = updateDto.EscalationId1;
            contract.EscalationId2 = updateDto.EscalationId2;
            contract.EscalationId3 = updateDto.EscalationId3;
            contract.TriggerDaysEscalation1 = updateDto.TriggerDaysEscalation1;
            contract.TriggerDaysEscalation2 = updateDto.TriggerDaysEscalation2;
            contract.TriggerDaysEscalation3 = updateDto.TriggerDaysEscalation3;

            _context.MasterEscalationMatrixContracts.Update(contract);
            return _context.SaveChanges();


        }
    }
}
