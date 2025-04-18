using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using CMS.Identity.Context;
using CMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CMS.Persistence.Repositories
{
   
    public class MasterEmployeeRepository : IMasterEmployeeRepository
    {
        private readonly CMSIdentityDbContext _idbContext;
        public MasterEmployeeRepository(CMSIdentityDbContext idbContext)
        {
            _idbContext = idbContext;
        }

        public async Task<IEnumerable<MasterEmployee>> GetAllEmployeesAsync(string unit, string searchTerm, int pageNumber, int pageSize)
        {
            var query = _idbContext.Users.AsQueryable();
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

            return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        }

        public async Task<MasterEmployee> GetEmployeeByIdAsync(int id)
        {
            var gotUser = await _idbContext.Users.FindAsync(id);
            if(gotUser == null)
                throw new Exception("Employee not found. Failed :(");
            return gotUser;
        }

        public async Task<MasterEmployee> AddEmployeeAsync(MasterEmployee employee)
        {
            await _idbContext.Users.AddAsync(employee);
            if(await _idbContext.SaveChangesAsync() > 0)
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
            var employee = await _idbContext.Users.FirstOrDefaultAsync(m => m.ValueId == id);
            if (employee == null)
            {
                throw new Exception("Employee not Found. :(");
            }
            employee.IsDeleted = true;
            _idbContext.Users.Update(employee);
            if (await _idbContext.SaveChangesAsync() > 0)
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
            var checkEmp = await _idbContext.Users.FirstOrDefaultAsync(m => m.ValueId == id);
            if (checkEmp == null)
            {
                throw new Exception("Employee not Found. :(");
            }
            _idbContext.Entry(employee).State = EntityState.Modified;
            if (await _idbContext.SaveChangesAsync() > 0)
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
