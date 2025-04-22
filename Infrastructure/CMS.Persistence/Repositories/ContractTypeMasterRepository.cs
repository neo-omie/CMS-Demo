using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Persistence.Repositories
{
    public class ContractTypeMasterRepository : IContractTypeMasterRepository
    {
        private readonly CMSDbContext _context;

        public ContractTypeMasterRepository(CMSDbContext context)
        {
            _context = context;
        }
        public async Task<ContractTypeMasters> AddContractAsync(ContractTypeMasters ctp)
        {
            await _context.contracts.AddAsync(ctp);
            if (await _context.SaveChangesAsync() > 0)
            {
                return ctp;
            }
            else
            {
                throw new Exception("Contract not added. Failed :(");
            }
        }

        public async Task<bool> DeletContract(int id)
        {
            var contr = await _context.contracts.FirstOrDefaultAsync(dl => dl.ValueId == id);
            if (contr==null)
            {
                throw new Exception("Contract not Found . :(");
            }
            contr.IsDeleted = true;
            _context.contracts.Update(contr);
            if (await _context.SaveChangesAsync()>0)
            {
                return true;
            }
            else
            {
                throw new Exception("Contract not deleted . failed");
            }
            
        }

        public async Task<IEnumerable<ContractTypeMasters>> GetAllContractAsync( int pageNumber, int pageSize)
        {
            var query = _context.contracts.AsQueryable();

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<ContractTypeMasters> GetContractById(int id)
        {
            var gotCont = await _context.contracts.FirstOrDefaultAsync(gc => gc.ValueId == id);
            if (gotCont == null)
                throw new Exception("contract not found . Failed ;(");
            return gotCont;
            
        }

        public async Task<ContractTypeMasters> UpdateContractAsync(int id, ContractTypeMasters ctp)
        {
            var checkCont = await _context.contracts.FirstOrDefaultAsync(up => up.ValueId == id);
            if (checkCont == null)
            {
                throw new Exception("contract not found :(");
            }
            checkCont.Status = ctp.Status;
            checkCont.ContractTypeName = ctp.ContractTypeName;
            _context.contracts.Update(checkCont);
            await _context.SaveChangesAsync();
            return checkCont;
        }
    }
}
