using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CMS.Persistence.Repositories
{
   
    public class MasterEmployeeRepository : IMasterEmployeeRepository
    {
        private readonly CMSDbContext _context;
        public MasterEmployeeRepository(CMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MasterEmployee>> GetAllEmployeesAsync(string unit, string searchTerm, int pageNumber, int pageSize)
        {
            var query = _context.MasterEmployees.AsQueryable();
            if(!string.IsNullOrEmpty(unit) && unit != "All")
            {
                query = query.Where(e => e.EmployeeLocation == unit);
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e => e.ValueId.Contains(searchTerm) || e.EmployeeName.Contains(searchTerm));
            }

            return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        }

        public async Task<MasterEmployee> GetEmployeeByIdAsync(string id)
        {
            return await _context.MasterEmployees.FindAsync(id);
        }

        public async Task AddEmployeeAsync(MasterEmployee employee)
        {
            await _context.MasterEmployees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(MasterEmployee employee)
        {
            employee.IsDeleted = true; 
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmployeeAsync(MasterEmployee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
