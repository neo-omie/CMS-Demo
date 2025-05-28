using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.ContractTypeMaster.Query;
using CMS.Domain.Constants;
using CMS.Domain.Entities;
using CMS.Domain.Entities.CompanyMaster;
using CMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace CMS.Persistence.Repositories
{
    public class ContractTypeMasterRepository : IContractTypeMasterRepository
    {
        private readonly CMSDbContext _context;
        private readonly ICacheService _cacheService;
        private readonly string cacheKey = $"{typeof(ContractTypeMasters)}";

        public ContractTypeMasterRepository(CMSDbContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }
        //adding contract
        public async Task<ContractTypeMasters> AddContractAsync(ContractTypeMasters ctp,string empCode)
        {
            //await _context.contracts.AddAsync(ctp);
            //if (await _context.SaveChangesAsync() > 0)
            //{
            //    return ctp;
            //}
            //else
            //{
            //    throw new Exception("Contract not added. Failed :(");
            //}
            string sql = "EXEC SP_AddContractType  @ContractTypeName={0},@Status={1}";
            int result = await _context.Database.ExecuteSqlRawAsync(sql, ctp.ContractTypeName, ctp.Status, 0);

            var contractType =await _context.contracts.OrderBy(x=>x.ValueId).LastAsync();
            if (result > 0)
            {
                string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
                await _context.Database.ExecuteSqlRawAsync(query, contractType.ValueId, TableList.MasterContractType, $"New ContractType '{ctp.ContractTypeName}' has been Added by '{empCode}'", empCode, LogStatus.Created);

                return ctp;
            }
            else
            {
                throw new Exception("Contract not added. Failed :(");
            }
        }

        //deleting contract
        public async Task<bool> DeletContract(int id,string empCode)
        {
            var contr = await _context.contracts.FirstOrDefaultAsync(dl => dl.ValueId == id);
            if (contr==null)
            {
                throw new Exception("Contract not Found . :(");
            }

            string sql = "EXEC SP_DeleteContractyId @ValId={0}";
            var compbyId = await _context.Database.ExecuteSqlRawAsync(sql, id);
            if (compbyId > 0)
            {
                string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
                await _context.Database.ExecuteSqlRawAsync(query, id, TableList.MasterContractType, $"ContractType '{contr.ContractTypeName}' has been Deleted by '{empCode}' ", empCode, LogStatus.Deleted);

                return true;
            }
            return false;
            //contr.IsDeleted = true;
            //_context.contracts.Update(contr);
            //if (await _context.SaveChangesAsync()>0)
            //{
            //    return true;
            //}
            //else
            //{
            //    throw new Exception("Contract not deleted . failed");
            //}
        }

        //getting all contract
        public async Task<IEnumerable<GetAllContractTypesDTO>> GetAllContractAsync( int pageNumber, int pageSize)
        {
            var totalRecords = await _context.contracts.Where(x => x.IsDeleted == false).CountAsync();
            var query = _context.contracts.AsQueryable();
            string sql = "EXEC SP_GetAllContractsTypes @PageNumber= {0}, @PageSize={1}";
            var rawData = await _context.contracts
            .FromSqlRaw(sql, pageNumber, pageSize)
            .AsNoTracking()
            .ToListAsync();
            var res =  rawData.Select(c => new GetAllContractTypesDTO
            {
                ValueId = c.ValueId,
                ContractTypeName = c.ContractTypeName,
                Status = c.Status,
                IsDeleted = c.IsDeleted,
                TotalRecords = totalRecords
            }).ToList();
            //IEnumerable<GetAllContractTypesDTO> res = null;
            //if (!_cacheService.TryGet(cacheKey, out IReadOnlyList<ContractTypeMasters> cachedList))
            //{
            //    cachedList = await _context.Set<ContractTypeMasters>().ToListAsync();
            //    res = cachedList.Select(c => new GetAllContractTypesDTO
            //    {
            //        ValueId = c.ValueId,
            //        ContractTypeName = c.ContractTypeName,
            //        Status = c.Status,
            //        IsDeleted = c.IsDeleted,
            //        TotalRecords = totalRecords
            //    });
            //    _cacheService.Set(cacheKey, res);
            //    _cacheService.Set(cacheKey, cachedList);
            //}

            return res;
        }
        public async Task<ContractTypeMasters> GetContractById(int id)
        {
            var gotCont = await _context.contracts.FirstOrDefaultAsync(gc => gc.ValueId == id);
            if (gotCont == null)
                throw new Exception("contract not found . Failed ;(");
            return gotCont;
        }

        public async Task<ContractTypeMasters> UpdateContractAsync(int id, ContractTypeMasters ctp,string empCode)
        {
            var checkCont = await _context.contracts.FirstOrDefaultAsync(up => up.ValueId == id);
            string contractNa = checkCont.ContractTypeName;
            if (checkCont == null)
            {
                throw new Exception("contract not found :(");
            }
            checkCont.Status = ctp.Status;
            checkCont.ContractTypeName = ctp.ContractTypeName;
            _context.contracts.Update(checkCont);
            if(await _context.SaveChangesAsync() > 0)
            {
                string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
                await _context.Database.ExecuteSqlRawAsync(query, id, TableList.MasterContractType, $"Contract type name '{contractNa}' has been Updated to '{ctp.ContractTypeName}' by '{empCode}' ", empCode, LogStatus.Updated);

                return checkCont;
            }
            throw new Exception($"For some reasons, contract has not been updated");
        }
    }
}
