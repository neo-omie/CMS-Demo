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

        public async Task<IEnumerable<MasterApostille>> GetAllMasterApostilleAsync(string searchTerm, int pageNumber, int pageSize)
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
            .Where(x => x.IsDeleted == false)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        }

        public async Task<MasterApostille> GetMasterApostilleByIdAsync(int id)
        {
            var query = await _context.MasterApostilles.FirstOrDefaultAsync(m => m.ValueId == id);
            if (query == null)
                throw new Exception("Master Apostille not found. Failed :(");
            return query;
        }

        public async Task<MasterApostille> AddMasterApostilleAsync(MasterApostille masterApostille)
        {
            await _context.MasterApostilles.AddAsync(masterApostille);
            if (await _context.SaveChangesAsync() > 0)
            {
                return masterApostille;
            }
            else
            {
                throw new Exception("Master Apostille not added. Failed :(");
            }
        }

        public async Task<bool> DeleteMasterApostilleAsync(int id)
        {
            var apostille = await _context.MasterApostilles.FirstOrDefaultAsync(m => m.ValueId == id);
            if (apostille == null)
                throw new Exception("Apostille not found. Failed :(");

            apostille.IsDeleted = true;
            _context.MasterApostilles.Update(apostille);
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            else
            {
                throw new Exception("Master Apostille not deleted. Failed :(");
            }
        }

        public async Task<MasterApostille> UpdateMasterApostilleAsync(int id, MasterApostille masterApostille)
        {
            var checkApostille = await _context.MasterApostilles.FirstOrDefaultAsync(m => m.ValueId == id);
            if (checkApostille == null)
                throw new Exception("Master Apostille not found. Failed :(");
            _context.Entry(checkApostille).CurrentValues.SetValues(masterApostille);
            if (await _context.SaveChangesAsync() > 0)
            {
                return checkApostille;
            }
            else
            {
                throw new Exception("Master Apostille not updated. Failed :(");
            }
        }
    }
}
