using CMS.Application.Contracts.Persistence;
using CMS.Application.Exceptions;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using Microsoft.AspNetCore.Identity;
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

            var hasher = new PasswordHasher<MasterEmployee>();
            var hashedPswd = hasher.HashPassword(null, employee.Password);
            employee.Password = hashedPswd;
            employee.LastPasswordChanged = DateTime.Now;
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
            //_context.MasterEmployees.Update(employee);
            checkEmp.EmployeeName = employee.EmployeeName;
            checkEmp.Password = employee.Password;
            checkEmp.EmployeeCode = employee.EmployeeCode;
            checkEmp.Unit = employee.Unit;
            checkEmp.DepartmentId = employee.DepartmentId;
            checkEmp.EmployeeMobile = employee.EmployeeMobile;
            checkEmp.Email = employee.Email;
            checkEmp.EmployeeExtension = employee.EmployeeExtension;
            if (await _context.SaveChangesAsync() > 0)
            {
                return employee;
            }
            else
            {
                throw new Exception("Employee not updated. Failed :(");
            }
        }

        public async Task<IEnumerable<MasterEmployee>> GetEmployeesByDepartmentIdAndEmployeeDetails(int departmentId, string inpQuery)
        {
            var checkEmployees = await _context.MasterEmployees.Where(me => me.DepartmentId == departmentId).ToListAsync();
            if(checkEmployees == null)
            {
                throw new NotFoundException($"Employees not found. Probably department id is incorrect.");
            }
            var foundEmployees = await _context.MasterEmployees.Where(me => (me.DepartmentId == departmentId) && (me.EmployeeName.Contains(inpQuery) || me.EmployeeCode.Contains(inpQuery))).ToListAsync();
            return foundEmployees;
        }
    }
}
