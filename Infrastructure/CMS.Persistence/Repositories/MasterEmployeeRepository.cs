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

        public async Task<IEnumerable<MasterEmployee>> GetAllEmployeesAsync(string unit = "All", string searchTerm = "", int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.MasterEmployees.AsQueryable();
            if(!string.IsNullOrEmpty(unit) && unit != "All")
            {
                query = query.Where(e => e.EmployeeLocation == unit);
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e => e.ValueId == Convert.ToInt32(searchTerm) || e.EmployeeName.Contains(searchTerm));
            }

            return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        }

        public async Task<MasterEmployee> GetEmployeeByIdAsync(int id)
        {
            return await _context.MasterEmployees.FindAsync(id);
        }

        public async Task<MasterEmployee> AddEmployeeAsync(MasterEmployee employee)
        {
            await _context.MasterEmployees.AddAsync(employee);
            if(await _context.SaveChangesAsync()>0)
            {
                return employee;
            }
            else
            {
                throw new Exception("Employee not added. Failed :(");
            }
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.MasterEmployees.FirstOrDefaultAsync(m => m.ValueId == id);
            if (employee == null)
            {
                throw new Exception("Employee not Found. :(");
            }
            employee.IsDeleted = true;
            _context.MasterEmployees.Update(employee);
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            else
            {
                throw new Exception("Employee not deleted. Failed :(");
            }
        }

        public async Task<MasterEmployee> UpdateEmployeeAsync(int id,MasterEmployee employee)
        {
            var checkEmp = await _context.MasterEmployees.FirstOrDefaultAsync(m => m.ValueId == id);
            if (checkEmp == null)
            {
                throw new Exception("Employee not Found. :(");
            }
            _context.Entry(employee).State = EntityState.Modified;
            if (await _context.SaveChangesAsync() > 0)
            {
                return employee;
            }
            else
            {
                throw new Exception("Employee not updated. Failed :(");
            }
        }
    }
}
