using CMS.Application.Contracts.Persistence;
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
    public class CompanyCascadeRepository : ICompanyCascadeRepository
    {
        readonly CMSDbContext _context;
        public CompanyCascadeRepository(CMSDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ListOfCountries>> GetCountries()
        {
            return await _context.Countries.ToListAsync();
        }
        public async Task<IEnumerable<ListOfStates>> GetStates(int countryId)
        {
            return await _context.States.Where(s => s.CountryId == countryId).ToListAsync();
        }
        public async Task<IEnumerable<ListofCity>> GetCities(int stateId)
        {
            return await _context.Cities.Where(s => s.StateId == stateId).ToListAsync();
        }
    }
}
