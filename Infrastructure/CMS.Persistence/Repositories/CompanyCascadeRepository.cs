using CMS.Application.Contracts.Persistence;
using CMS.Application.Exceptions;
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
            string sql = "EXEC SP_GetCountries";
            var countries = _context.Countries.FromSqlRaw(sql).ToList();
            return countries;
        }
        public async Task<IEnumerable<ListOfStates>> GetStates(int countryId)
        {
            string sql = "EXEC SP_GetStates @CountryID = {0}";
            var states = _context.States.FromSqlRaw(sql, countryId).ToList();
            if (states.Count() <= 0)
                throw new NotFoundException($"States with Country Id {countryId} not found.");
            return states;
        }
        public async Task<IEnumerable<ListofCity>> GetCities(int stateId)
        {
            string sql = "EXEC SP_GetCities @StateID = {0}";
            var cities = _context.Cities.FromSqlRaw(sql, stateId).ToList();
            if (cities == null)
                throw new NotFoundException($"Cities with State Id {stateId} not found.");
            return cities;
        }
        public async Task<ListOfCountries> GetCountryById(int id)
        {
            var foundCountry = await _context.Countries.FirstOrDefaultAsync(c => c.CountryId == id);
            if(foundCountry == null)
                throw new NotFoundException($"Country not found.");
            return foundCountry;
        }
        public async Task<ListOfStates> GetStateById(int id)
        {
            var foundState = await _context.States.FirstOrDefaultAsync(c => c.StateId == id);
            if (foundState == null)
                throw new NotFoundException($"State not found.");
            return foundState;
        }
        public async Task<ListofCity> GetCityById(int id)
        {
            var foundCity = await _context.Cities.FirstOrDefaultAsync(c => c.CityId == id);
            if (foundCity == null)
                throw new NotFoundException($"City not found.");
            return foundCity;
        }
    }
}
