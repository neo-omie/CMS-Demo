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

        public async Task<(IEnumerable<MasterEmployee> Data, int TotalCount)> GetAllEmployeesAsync(int pageNumber, int pageSize, string? unit, string? searchTerm )
        {
            var query = _context.MasterEmployees.AsQueryable();
            if(!string.IsNullOrEmpty(unit) && unit != "All")
            {
                query = query.Where(e => e.Unit == unit);
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var isInt = Int32.TryParse(searchTerm, out int checkID);
                if (isInt)
                    query = query.Where(e => e.ValueId == checkID);
                else
                    query = query.Where(e => e.EmployeeName.Contains(searchTerm));
            }

            query = query.Where(x => x.IsDeleted == false);

            int totalCount = await query.CountAsync();

            var data= await query
            .Where(x => x.IsDeleted == false)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            return (data, totalCount);
        }

        public async Task<MasterEmployee> GetEmployeeByIdAsync(int id)
        {
            var gotUser = await _context.MasterEmployees.FirstOrDefaultAsync(me => me.ValueId == id);
            if(gotUser == null)
                throw new Exception("Employee not found. Failed :(");
            return gotUser;
        }

        public async Task<MasterEmployee> AddEmployeeAsync(MasterEmployee employee)
        {
            await _context.MasterEmployees.AddAsync(employee);
            if(await _context.SaveChangesAsync() > 0)
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
            var employee = await _context.MasterEmployees.FirstOrDefaultAsync(me => me.ValueId == id);
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
            var checkEmp = await _context.MasterEmployees.FirstOrDefaultAsync(me => me.ValueId == id);
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
