using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using CMS.Domain.Entities;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using CMS.Application.Features.MasterApostilles.ApostilleDtos;
using CMS.Application.Exceptions;
using Microsoft.Data.SqlClient;
using System.Data;
using CMS.Domain.Constants;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Reflection.Emit;

namespace CMS.Persistence.Repositories
{
    public class MasterApostilleRepository : IMasterApostilleRepository
    {
        private readonly CMSDbContext _context;

        public MasterApostilleRepository(CMSDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<MasterApostille> Data, int TotalCount)> GetAllMasterApostilleAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            var query = _context.MasterApostilles.AsQueryable();
            int totalCount = await query.Where(a => a.IsDeleted == false).CountAsync();
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("Page size must be greater than 0.");
            }

            string sql = "EXEC SP_GetAllApostilles @PageNumber = {0}, @PageSize = {1}, @searchTerm={2}";
            var data = await _context.MasterApostilles.FromSqlRaw(sql, pageNumber, pageSize, searchTerm).ToListAsync();

            return (data, totalCount);
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

        public async Task<MasterApostille> AddMasterApostilleAsync(MasterApostille masterApostille, string empCode)
        {
            //await _context.MasterApostilles.AddAsync(masterApostille);
            //if (await _context.SaveChangesAsync() > 0)
            //{
            //    return masterApostille;
            //}
            //else
            //{
            //    throw new Exception("Master Apostille not added. Failed :(");
            //}
            string sql = "EXEC sp_AddApostille @ApostilleName, @Status, @IsDeleted, @ValueId OUTPUT";
            var parameters = new[]
            {
                new SqlParameter("@ApostilleName",masterApostille.ApostilleName),
                new SqlParameter("@Status",masterApostille.Status),
                new SqlParameter("@IsDeleted",masterApostille.IsDeleted),
                new SqlParameter
                {
                    ParameterName="@ValueId",
                    SqlDbType=SqlDbType.Int,
                    Direction=ParameterDirection.Output
                }
            };
            var addingApostille = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
            var getApostille = _context.MasterApostilles.OrderBy(x => x.ValueId).Last();
            if (addingApostille > 0)
            {
                masterApostille.ValueId = (int)parameters[3].Value;

                string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
                await _context.Database.ExecuteSqlRawAsync(query, getApostille.ValueId, TableList.MasterApostille, $"New Apostille {getApostille.ApostilleName} has been Added by " + empCode, empCode, LogStatus.Created);
                return masterApostille;
            }
            throw new Exception("Failed to add apostille");
        }

        public async Task<bool> DeleteMasterApostilleAsync(int id,string empCode)
        {
            //var apostille = await _context.MasterApostilles.FirstOrDefaultAsync(m => m.ValueId == id);
            //if (apostille == null)
            //    throw new Exception("Apostille not found. Failed :(");

            //apostille.IsDeleted = true;
            //_context.MasterApostilles.Update(apostille);
            //if (await _context.SaveChangesAsync() > 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    throw new Exception("Master Apostille not deleted. Failed :(");
            //}
            var getApostille = await _context.MasterApostilles.FindAsync(id);
            var sql = "EXEC sp_DeleteApostille @Id={0}";
            var deleteApostille = await _context.Database.ExecuteSqlRawAsync(sql, id);
            if (deleteApostille > 0)
            {
                string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
                await _context.Database.ExecuteSqlRawAsync(query, id, TableList.MasterApostille, $"Apostille {getApostille.ApostilleName} has been Deleted by " + empCode, empCode, LogStatus.Deleted);
               
                return true;
            }
            return false;
        }

        public async Task<MasterApostille> UpdateMasterApostilleAsync(int id, MasterApostille masterApostille,string empCode)
        {
            //var checkApostille = await _context.MasterApostilles.FirstOrDefaultAsync(m => m.ValueId == id);
            //if (checkApostille == null)
            //    throw new Exception("Master Apostille not found. Failed :(");

            ////_context.Entry(checkApostille).CurrentValues.SetValues(masterApostille);
            //_context.MasterApostilles.Update(masterApostille);

            //checkApostille.ApostilleName = masterApostille.ApostilleName;
            //checkApostille.Status = masterApostille.Status;

            //if (await _context.SaveChangesAsync() > 0)
            //{
            //    return checkApostille;
            //}
            //else
            //{
            //    throw new Exception("Master Apostille not updated. Failed :(");
            //}
            var getApostille = await _context.MasterApostilles.FindAsync(id);

            var sql = "EXEC sp_UpdateApostille @Id={0}, @ApostilleName={1}, @Status={2}";
            var parameters = new[]
            {
                new SqlParameter("@Id",id),
                new SqlParameter("@ApostilleName",masterApostille.ApostilleName),
                new SqlParameter("@Status",masterApostille.Status)
            };

            var updateApostille = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
            if (updateApostille > 0)
            {
                var checkApostille = await _context.MasterApostilles.FirstOrDefaultAsync(m => m.ValueId == id);
                if (checkApostille != null)
                {
                    string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
                    await _context.Database.ExecuteSqlRawAsync(query, id, TableList.MasterApostille, $"Apostille {getApostille.ApostilleName} has been updated by " + empCode, empCode, LogStatus.Updated);

                    return checkApostille;
                }
                throw new Exception("Apostille ValueId not found after update");
            }
            throw new Exception("Failed to update apostille");
        }
    }
}
