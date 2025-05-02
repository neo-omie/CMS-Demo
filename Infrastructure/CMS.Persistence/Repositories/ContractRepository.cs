using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Exceptions;
using CMS.Application.Features.Contracts.Queries.GetAllContracts;
using CMS.Application.Features.Contracts.Queries.GetContractById;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CMS.Persistence.Repositories
{
    public class ContractRepository : IContractRepository
    {
        readonly CMSDbContext _context;
        public ContractRepository(CMSDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<GetAllContractsDto>> GetAllContractsAsync(int pageNumber, int pageSize)
        {
            int totalRecords = await _context.ContractsEntity.Where(x => x.IsDeleted == false).CountAsync();
            string sql = "EXEC SP_GetAllContractsEntity @PageNumber = {0}, @PageSize = {1}";
            var allContracts = await _context.GetContractsDtos.FromSqlRaw(sql, pageNumber, pageSize).ToListAsync();
            return allContracts;
        }

        public async Task<GetContractByIdDto> GetContractByIdAsync(int id)
        {
            string sql = "EXEC SP_GetContractEntityByID @ID = {0}";
            var findingContract = await _context.GetContractByIdDtos.FromSqlRaw(sql, id).AsNoTracking().ToListAsync();
            var foundContract = findingContract.FirstOrDefault();
            if(foundContract == null)
            {
                throw new NotFoundException($"Contract with ID {id} not found");
            }
            return foundContract;

        }
        public async Task<Contract> AddContractAsync(Contract cp)
        {
            var addedContract = await _context.ContractsEntity.AddAsync(cp);
            if(await _context.SaveChangesAsync() > 0)
                return cp;
            throw new Exception("For some reasons, contract has not been added.");
        }

        public async Task<bool> DeleteContractAsync(int id)
        {
            var foundContract = await _context.ContractsEntity.FirstOrDefaultAsync(ce => ce.ContractId == id);
            if (foundContract == null)
            {
                throw new NotFoundException($"Contract with id {id} not found. Please enter correct id");
            }
            foundContract.IsDeleted = true;
            _context.ContractsEntity.Update(foundContract);
            if(await _context.SaveChangesAsync() > 0)
                return true;
            return false;
        }


        public async Task<bool> UpdateContractAsync(int id, Contract cp)
        {
            var foundContract = await _context.ContractsEntity.FirstOrDefaultAsync(ce => ce.ContractId == id);
            if (foundContract == null)
            {
                throw new NotFoundException($"Contract with id {id} not found. Please enter correct id");
            }
            cp.ContractId = foundContract.ContractId;
            _context.ContractsEntity.Update(cp);
            if (await _context.SaveChangesAsync() > 0)
                return true;
            return false;
        }
    }
}
