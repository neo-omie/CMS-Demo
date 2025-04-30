using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOU;
using CMS.Application.Features.MasterCompanies;
using CMS.Domain.Entities.CompanyMaster;
using CMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
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
            await _context.MasterCompanies.AddAsync(masterCompany);
            if (await _context.SaveChangesAsync()>0)
            {
                return masterCompany;
            }
            else
            {
                throw new Exception("Company not added. Failed :(");
            }
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var company = await _context.MasterCompanies.FirstOrDefaultAsync(cm => cm.ValueId == id);
            if (company == null)
            {
                throw new Exception("Company  not Found. :(");
            }
            company.IsDeleted = true;
            _context.MasterCompanies.Update(company);
            if (await _context.SaveChangesAsync()>0)
            {
                return true;
            }
            else
            {
                throw new Exception("Company not deleted . failed :(");
            }
        }

        public async Task<IEnumerable<GetMastersDTO>> GetAllCompanyDetailsAsync( string? searchTerm, int pageNumber, int pageSize)
        {
            var totalRecords = await _context.MasterCompanies.Where(x => x.IsDeleted == false).CountAsync();
            var query = _context.MasterCompanies.AsQueryable();
           
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e => e.CompanyName.Contains(searchTerm));
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
            string sql = "EXEC SP_GetAllCompanies @PageNumber= {0}, @PageSize={1}, @searchTerm= {3}";
            var res = _context.MasterCompanies.FromSqlRaw(sql, pageNumber, pageSize, searchTerm)
                .Select(a => new GetMastersDTO
            {
                ValueId = a.ValueId,
                CompanyName = a.CompanyName,
                CompanyLocation = a.city.City,
                status = a.CompanyStatus,
                TotalRecords = totalRecords
            }); ;

            return res;
        }

        public async Task<MasterCompany> GetCompanyByIdAsync(int id)
        {
            var gotComp = await _context.MasterCompanies.FirstOrDefaultAsync(cm => cm.ValueId == id);
            if (gotComp==null)
                throw new Exception("Company not found. Failed :(");
            return gotComp;

        }

        public async Task<MasterCompany> UpdateCompanyAsync(int id, MasterCompany masterCompany)
        {
            var checkComp = await _context.MasterCompanies.FirstOrDefaultAsync(cm => cm.ValueId == id);
            if (checkComp==null)
            {
                throw new Exception("Company Not found :(");
            }
            _context.Entry(masterCompany).State = EntityState.Modified;
            if (await _context.SaveChangesAsync()>0)
            {
                return masterCompany;
            }
            else {
                throw new Exception("Company not updated. Failed :(");
            }
        }
    }
}
