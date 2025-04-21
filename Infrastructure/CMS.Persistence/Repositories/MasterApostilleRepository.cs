using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using CMS.Domain.Entities;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;

namespace CMS.Persistence.Repositories
{
    public class MasterApostilleRepository : IMasterApostilleRepository
    {
        private readonly CMSDbContext _context;

        public MasterApostilleRepository(CMSDbContext context)
        {
            _context = context;
        }

        public Task<MasterApostille> AddMasterApositlleAsync(MasterApostille masterApostille)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMasterApostilleAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MasterApostille>> GetAllMasterApostilleAsync(string unit, string searchTerm, int pageNumber, int pageSize)
        {
            var query = _context.MasterApostilles.AsQueryable();
            if(!string.IsNullOrEmpty(searchTerm))
            {
                var IsInt= Int32.TryParse(searchTerm, out int checkId);
                if (IsInt)
                {
                    query=query.Where(m=>m.ValueId == checkId);
                }
                else
                {
                    query=query.Where(e=>e.ApostilleName.Contains(searchTerm)); 
                }  
            }
            return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        }

        public Task<MasterApostille> GetMasterApostilleByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MasterApostille> UpdateMasterApostilleAsync(int id, MasterApostille masterApostille)
        {
            throw new NotImplementedException();
        }
    }
}
