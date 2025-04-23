using CMS.Domain.Entities.CompanyMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Contracts.Persistence
{
   public  interface IMasterCompanyRepository
    {
        Task<IEnumerable<MasterCompany>> GetAllCompanyDetailsAsync( string searchTerm, int pageNumber, int pageSize);

        Task<MasterCompany> GetCompanyByIdAsync(int id);

        Task<MasterCompany> AddCompanyAsync(MasterCompany masterCompany);

        Task<MasterCompany> UpdateCompanyAsync(int id, MasterCompany masterCompany);

        Task<bool> DeleteCompanyAsync(int id);
    }
}
