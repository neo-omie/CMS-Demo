using CMS.Application.Exceptions;
using CMS.Application.Features.MasterEscalationMatrixContracts.Command;
using CMS.Application.Features.MasterEscalationMatrixContracts;
using CMS.Persistence.Context;
using CMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CMS.Application.Features.EscalationMatrixMouMaster;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.EscalationMatrixMouMaster.Commands.UpdateEscalationMatrixMou;

namespace CMS.Persistence.Repositories
{
    public class MasterEscalationMatrixMouRepository : IMasterEscalationMatrixMouRepository
    {
        private readonly CMSDbContext _context;
        public MasterEscalationMatrixMouRepository(CMSDbContext context)
        {
            _context = context;
        }



        public async Task<IEnumerable<EscalationMatrixMoutDto>> GetAllEscalationMatrixMou(int pageNumber, int pageSize)
        {

            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("Page size must be greater than 0.");
            }


            int totalRecords = await _context.MasterEscalationMatrixMous.CountAsync();
            string sql = "EXEC SP_GetAllEscalationMatrixMOUs @PageNumber = {0}, @PageSize = {1}";
            var allEscalations = _context.GetEscalationMatrixMouDtos.FromSqlRaw(sql, pageNumber, pageSize);
            return allEscalations;
        }

        public async Task<EscalationMatrixMoutDto> GetEscalationMatrixMou(int valueId)
        {
            var contract = await _context.MasterEscalationMatrixMous
                .FirstOrDefaultAsync(x => x.MatrixMouId == valueId);
            if (contract == null)
            {
                throw new NotFoundException($"Escalation matrix contract with id {valueId} not found.");
            }
            return await _context.MasterEscalationMatrixMous.Where(x => x.MatrixMouId == valueId).Select(x => new EscalationMatrixMoutDto
            {
                MatrixMouId = x.MatrixMouId,
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

        public async Task<int> UpdateMatrixMou(int valueId, UpdateEscalationMatrixMouDto updateDto)
        {
            var mou = await _context.MasterEscalationMatrixMous.FirstOrDefaultAsync(x => x.MatrixMouId == valueId);
            if (mou == null)
            {
                throw new NotFoundException("Escalation Mou not Found");
            }
            mou.EscalationId1 = updateDto.EscalationId1;
            mou.EscalationId2 = updateDto.EscalationId2;
            mou.EscalationId3 = updateDto.EscalationId3;
            mou.TriggerDaysEscalation1 = updateDto.TriggerDaysEscalation1;
            mou.TriggerDaysEscalation2 = updateDto.TriggerDaysEscalation2;
            mou.TriggerDaysEscalation3 = updateDto.TriggerDaysEscalation3;

            _context.MasterEscalationMatrixMous.Update(mou);
            return _context.SaveChanges();


        }
    }
}
