using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOU;
using CMS.Application.Features.MasterCompanies;
using CMS.Domain.Entities.CompanyMaster;
using CMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Persistence.Repositories
{
    public class MasterCompanyRepository : IMasterCompanyRepository
    {
        private readonly CMSDbContext _context;

        public MasterCompanyRepository(CMSDbContext context)
        {
            _context = context;
        }

        public async Task<MasterCompany> AddCompanyAsync(MasterCompany masterCompany)
        {
            string sql = "EXEC SP_AddCompany @CompanyName={0},@PocName ={1}, @CompanyStatus ={2},@PocContactNumber={3},@PocEmailId={4},@CompanyAddressLine1={5}, @CompanyAddressLine2 ={6},@CompanyAddressLine3 ={7}, @Zipcode={8},@CompanyContactNo={9}, @CompanyEmailId ={10},@CompanyWebsiteUrl ={11}, @CompanyBankName ={12},@GSTno={13}, @BankAccNo={14},@MSMERegistrationNo={15}, @IFSCCode={16}, @PanNo ={17}, @CountryId={18}, @StateId= {19}, @CityId={20}, @IsDeleted={21}";
            int result = await _context.Database.ExecuteSqlRawAsync(sql, masterCompany.CompanyName, masterCompany.PocName, masterCompany.CompanyStatus, masterCompany.PocContactNumber, masterCompany.PocEmailId, masterCompany.CompanyAddressLine1, masterCompany.CompanyAddressLine2, masterCompany.CompanyAddressLine3, masterCompany.Zipcode, masterCompany.CompanyContactNo, masterCompany.CompanyEmailId, masterCompany.CompanyWebsiteUrl, masterCompany.CompanyBankName, masterCompany.GSTno, masterCompany.BankAccNo, masterCompany.MSMERegistrationNo, masterCompany.IFSCCode, masterCompany.PanNo, masterCompany.CountryId, masterCompany.StateId, masterCompany.CityId, 0);
            if (result > 0)
            {
                return masterCompany;
            }
            else
            {
                throw new Exception("Company not added. Failed :(");
            }
            //await _context.MasterCompanies.AddAsync(masterCompany);
            //if (await _context.SaveChangesAsync()>0)
            //{
            //    return masterCompany;
            //}
            //else
            //{
            //    throw new Exception("Company not added. Failed :(");
            //}
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var company = await _context.MasterCompanies.FirstOrDefaultAsync(cm => cm.ValueId == id);
            if (company == null)
            {
                throw new Exception("Company  not Found. :(");
            }


            string sql = "EXEC SP_DeleteCompanyById @ValId={0}";
            var compbyId = await _context.Database.ExecuteSqlRawAsync(sql, id);
            if (compbyId >0)
            {
                return true;
            }
            return false;
            
            //company.IsDeleted = true;
            //_context.MasterCompanies.Update(company);
            //if (await _context.SaveChangesAsync()>0)
            //{
            //    return true;
            //}
            //else
            //{
            //    throw new Exception("Company not deleted . failed :(");
            //}
        }

        public async Task<IEnumerable<GetMastersDTO>> GetAllCompanyDetailsAsync( string searchTerm, int pageNumber, int pageSize)
        {
            var totalRecords = await _context.MasterCompanies.Where(x => x.IsDeleted == false).CountAsync();
            var query = _context.MasterCompanies.AsQueryable();
           
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e => e.CompanyName.Contains(searchTerm));
            }
            if(searchTerm == null)
            {
                searchTerm = "";


            }
            //var result = _context.MasterCompanies.Skip((pageNumber - 1) * pageSize).Take(pageSize).Where(a=> a.IsDeleted == false)
            //    .Select(a => new GetMastersDTO
            //    {
            //        ValueId = a.ValueId,
            //        CompanyName = a.CompanyName,
            //        CompanyLocation = a.city.City,
            //        status = a.CompanyStatus,
            //        TotalRecords = totalRecords
            //    });
            //var result = query
            //.Where(x => x.IsDeleted == false)
            //.Skip((pageNumber - 1) * pageSize)
            //.Take(pageSize)
            string sql = "EXEC SP_GetAllCompanies @PageNumber= {0}, @PageSize={1}, @searchTerm= {2}";
            var res = _context.GetCompanyDtos.FromSqlRaw(sql, pageNumber, pageSize, searchTerm);
            return res;
        }

        public async Task<MasterCompany> GetCompanyByIdAsync(int id)
        {
            var gotComp = await _context.MasterCompanies.FirstOrDefaultAsync(cm => cm.ValueId == id);
            if (gotComp == null)
                throw new Exception("Company not found. Failed :(");

                string sql = "EXEC SP_GetCompanyById @ValId={0}";
            var compbyId = await _context.MasterCompanies.FromSqlRaw(sql, id).AsNoTracking().ToListAsync();
            var foundCompany = compbyId.FirstOrDefault();


                return foundCompany;

        }

        public async Task<MasterCompany> UpdateCompanyAsync(int id, MasterCompany masterCompany)
        {
            var checkComp = await _context.MasterCompanies.FirstOrDefaultAsync(cm => cm.ValueId == id);
            if (checkComp == null)
            {
                throw new Exception("Company Not found :(");
            }
            string sql = "EXEC SP_UpdateCompany @ValId={0}, @CompanyName={1},@PocName ={2}, @CompanyStatus ={3},@PocContactNumber={4},@PocEmailId={5},@CompanyAddressLine1 ={6}, @CompanyAddressLine2 ={7},@CompanyAddressLine3 ={8}, @Zipcode={9},@CompanyContactNo={10}, @CompanyEmailId ={11},@CompanyWebsiteUrl ={12}, @CompanyBankName ={13},@GSTno={14}, @BankAccNo={15},@MSMERegistrationNo={16}, @IFSCCode={17}, @PanNo ={18}, @CountryID={19}, @StateID={20}, @CityID={21}";
            int result = await _context.Database.ExecuteSqlRawAsync(sql, id, masterCompany.CompanyName, masterCompany.PocName, masterCompany.CompanyStatus, masterCompany.PocContactNumber, masterCompany.PocEmailId, masterCompany.CompanyAddressLine1, masterCompany.CompanyAddressLine2, masterCompany.CompanyAddressLine3, masterCompany.Zipcode, masterCompany.CompanyContactNo, masterCompany.CompanyEmailId, masterCompany.CompanyWebsiteUrl, masterCompany.CompanyBankName, masterCompany.GSTno, masterCompany.BankAccNo, masterCompany.MSMERegistrationNo, masterCompany.IFSCCode, masterCompany.PanNo, masterCompany.CountryId, masterCompany.StateId, masterCompany.CityId);
            //    var paramsList = new ArrayList();
            //    for (int i = 0; i < 35; i++)
            //    {
            //        paramsList[i] = masterCompany[i];
            //    }

            //    foreach(var m in masterCompany.)
            //    {

            //    }
            //    int result = await _context.Database.ExecuteSqlRawAsync(sql,...masterCompany);
            if (result > 0)
            {
                return masterCompany;
            }
            else
            {
                throw new Exception("Company not updated. Failed :(");
            }
        }
    }
    }
