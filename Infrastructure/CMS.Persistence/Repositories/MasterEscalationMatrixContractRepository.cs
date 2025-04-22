using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.ApprovalMatrixContract.Queries.GetAllApprovalMatrixContract;

using CMS.Application.Features.MasterEscalationMatrixContracts;
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


            var emContracts = _context.MasterEscalationMatrixContracts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new GetEscalationMatrixContractDto
                {
                    MatrixContractId = a.MatrixContractId,
                    DepartmentName = a.Department.DepartmentName,
                    Escalation1 = a.Escalation1,
                    Escalation2 = a.Escalation2,
                    Escalation3 = a.Escalation3,
                });

            return (emContracts, totalCount);
        }

        public Task<GetEscalationMatrixContractDto> GetEscalationMatrixContract(int valueId)
        {
            var contract = _context.MasterEscalationMatrixContracts
                .Where(c => c.MatrixContractId == valueId)
                .Select(
                c => new GetEscalationMatrixContractDto
                {
                    MatrixContractId = c.MatrixContractId,
                    DepartmentName = c.Department.DepartmentName,
                    Escalation1 = c.Escalation1,
                    Escalation2 = c.Escalation2,
                    Escalation3 = c.Escalation3
                }
                )
                .FirstOrDefaultAsync();
            return contract;
        }

        public Task<int> UpdateMatrixContract(int valueId)
        {

            return null;
        }
    }
}
