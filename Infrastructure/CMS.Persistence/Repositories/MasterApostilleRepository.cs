using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using CMS.Domain.Entities;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using CMS.Application.Features.MasterApostilles.ApostilleDtos;
using CMS.Application.Exceptions;

namespace CMS.Persistence.Repositories
{
    public class MasterApostilleRepository : IMasterApostilleRepository
    {
        private readonly CMSDbContext _context;

        public MasterApostilleRepository(CMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetAllApostilleDto>> GetAllMasterApostilleAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("Page size must be greater than 0.");
            }

            string sql = "EXEC SP_GetAllApostilles @PageNumber = {0}, @PageSize = {1}";
            var allApostilles = await _context.GetApostillesDtos.FromSqlRaw(sql, pageNumber, pageSize).ToListAsync();
            return allApostilles;
        }

        public async Task<MasterApostille> GetMasterApostilleByIdAsync(int id)
        {
            string sql = "EXEC SP_GetApostilleByID @id = {0}";
            var findingApostille = await _context.MasterApostilles.FromSqlRaw(sql, id).AsNoTracking().ToListAsync();
            var foundApostille = findingApostille.FirstOrDefault();
            if (foundApostille == null)
            {
                throw new NotFoundException($"Apostille with ID {id} not found.");
            }
            return foundApostille;
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
