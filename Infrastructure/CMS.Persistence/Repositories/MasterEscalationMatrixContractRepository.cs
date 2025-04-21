using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using CMS.Persistence.Context;

namespace CMS.Persistence.Repositories
{
    public class MasterEscalationMatrixContractRepository : IMasterEscalationMatrixContractRepository
    {
        private readonly CMSDbContext _context;
        public MasterEscalationMatrixContractRepository(CMSDbContext context)
        {
            _context = context;
        }

        public Task<MasterEscalationMatrixContract> GetEscalationMatrixContract(int valueId)
        {

            return null;

        }
        public Task<IEnumerable<MasterEscalationMatrixContract>> GetAllEscalationMatrixContract()
        {
            
            return null;

        }

        public Task<int> UpdateMatrixContract(int valueId)
        {

            return null;
        }
    }
}
