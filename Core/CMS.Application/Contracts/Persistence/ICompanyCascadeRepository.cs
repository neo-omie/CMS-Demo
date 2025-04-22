using CMS.Domain.Entities.CompanyMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Contracts.Persistence
{
     public interface ICompanyCascadeRepository
    {
        Task<IEnumerable<ListOfCountries>> GetCountries();
        Task<IEnumerable<ListOfStates>> GetStates(int countryId);
        Task<IEnumerable<ListofCity>> GetCities(int stateId);
    }
}
