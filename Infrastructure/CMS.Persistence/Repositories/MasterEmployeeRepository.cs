using System.Data;
using System.Text;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Exceptions;
using CMS.Application.Features.MasterEmployees.EmployeeDtos;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
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
        
        public async Task<(IEnumerable<MasterEmployee> Data, int TotalCount)> GetAllEmployeesAsync(int pageNumber, int pageSize, string? unit, string? searchTerm)
        {
            //var query = _context.MasterEmployees.AsQueryable();
            //if(!string.IsNullOrEmpty(unit) && unit != "All")
            //{
            //    query = query.Where(e => e.Unit == unit);
            //}
            //if (!string.IsNullOrEmpty(searchTerm))
            //{
            //    var isInt = Int32.TryParse(searchTerm, out int checkID);
            //    if (isInt)
            //        query = query.Where(e => e.ValueId == checkID);
            //    else
            //        query = query.Where(e => e.EmployeeName.Contains(searchTerm));
            //}

            //query = query.Where(x => x.IsDeleted == false);

            //int totalCount = await query.CountAsync();

            //var data= await query
            //.Where(x => x.IsDeleted == false)
            //.Skip((pageNumber - 1) * pageSize)
            //.Take(pageSize)
            //.ToListAsync();

            //return (data, totalCount);
            var query = _context.MasterEmployees.AsQueryable();
            int totalCount = await query.CountAsync();
            var data = await _context.MasterEmployees
                .FromSqlRaw("EXEC sp_GetAllEmployees @PageNumber= {0}, @PageSize={1}, @unit= {2}, @searchTerm= {3}", pageNumber, pageSize, unit, searchTerm)
                .ToListAsync();

            return(data, totalCount);
        }

        public async Task<MasterEmployee> GetEmployeeByIdAsync(int id)
        {
            //var gotUser = await _context.MasterEmployees.FirstOrDefaultAsync(me => me.ValueId == id);
            //if(gotUser == null)
            //    throw new Exception("Employee not found. Failed :(");
            //return gotUser;

            var employees = await _context.MasterEmployees
                            .FromSqlRaw("EXEC sp_GetEmployeeById @Id={0}", id)
                            .AsNoTracking()
                            .ToListAsync();

            var employee = employees.FirstOrDefault();

            if (employee == null)
                throw new Exception("Employee not found");

            return (employee);
        }

        public async Task<MasterEmployee> AddEmployeeAsync(MasterEmployee employee)
        {
            //var hasher = new PasswordHasher<MasterEmployee>();
            //var hashedPswd = hasher.HashPassword(null, employee.Password);
            //employee.Password = hashedPswd;
            //employee.LastPasswordChanged = DateTime.Now;
            //await _context.MasterEmployees.AddAsync(employee);
            //if(await _context.SaveChangesAsync() > 0)
            //{
            //    return employee;
            //}
            //else
            //{
            //    throw new Exception("Employee not added. Failed :(");
            //}

            var hasher = new PasswordHasher<MasterEmployee>();
            var hashedPswd = hasher.HashPassword(null, employee.Password);
            employee.Password = hashedPswd;
            employee.LastPasswordChanged = DateTime.Now;

            //var propertiesName = employee.GetType().GetProperties().Select(p => p.Name).ToArray();
            //var sb = new StringBuilder("EXEC sp_AddEmployee ");
            //for (int i = 0; i < propertiesName.Length; i++)
            //{
            //    if (propertiesName[i] == "ValueId") continue;
            //    sb.Append($"@{propertiesName[i]}, ");
            //}
            //sb.Append("@ValueId OUTPUT");
            //var sqlQuery = sb.ToString();
            //var parametersArray = new SqlParameter[propertiesName.Length];
            //var j = 0;
            //for (int i = 0; i < propertiesName.Length; i++)
            //{
            //    if (propertiesName[i] == "ValueId") continue;
            //    parametersArray[j] = new SqlParameter($"@{propertiesName[i]}", employee.GetType().GetProperty(propertiesName[i]).GetValue(employee));
            //    j++;
            //}
            //parametersArray[j] = new SqlParameter
            //{
            //    ParameterName = "@ValueId",
            //    SqlDbType = SqlDbType.Int,
            //    Direction = ParameterDirection.Output
            //};
            var parameters = new[]
            {
                new SqlParameter("@EmployeeName", employee.EmployeeName),
                new SqlParameter("@Password", employee.Password),
                new SqlParameter("@Role", employee.Role),
                new SqlParameter("@EmployeeCode", employee.EmployeeCode),
                new SqlParameter("@Unit", employee.Unit),
                new SqlParameter("@DepartmentId", employee.DepartmentId),
                new SqlParameter("@EmployeeMobile", employee.EmployeeMobile),
                new SqlParameter("@Email", employee.Email),
                new SqlParameter("@EmployeeExtension", employee.EmployeeExtension),
                new SqlParameter("@LastPasswordChanged", employee.LastPasswordChanged),
                new SqlParameter("@IsDeleted", employee.IsDeleted),
                new SqlParameter
                {
                    ParameterName = "@ValueId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                }
            };

            var result = await _context.Database.ExecuteSqlRawAsync("EXEC sp_AddEmployee @EmployeeName, @Password, @Role, @EmployeeCode, @Unit, @DepartmentId, @EmployeeMobile, @Email, @EmployeeExtension, @LastPasswordChanged, @IsDeleted, @ValueId OUTPUT", parameters);
            if (result > 0)
            {
                employee.ValueId = (int)parameters[11].Value;
                return employee;
            }

            throw new Exception("Failed to add employee");
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            //var employee = await _context.MasterEmployees.FirstOrDefaultAsync(me => me.ValueId == id);
            //if (employee == null)
            //{
            //    throw new Exception("Employee not Found. :(");
            //}
            //employee.IsDeleted = true;
            //_context.MasterEmployees.Update(employee);
            //if (await _context.SaveChangesAsync() > 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    throw new Exception("Employee not deleted. Failed :(");
            //}

            var affectedRows = await _context.Database.ExecuteSqlRawAsync("EXEC sp_DeleteEmployee {0}", id);
            return affectedRows > 0;
        }

        public async Task<MasterEmployee> UpdateEmployeeAsync(int id,MasterEmployee employee)
        {
            //var checkEmp = await _context.MasterEmployees.FirstOrDefaultAsync(me => me.ValueId == id);
            //if (checkEmp == null)
            //{
            //    throw new Exception("Employee not Found. :(");
            //}
            ////_context.MasterEmployees.Update(employee);
            //checkEmp.EmployeeName = employee.EmployeeName;
            //checkEmp.Password = employee.Password;
            //checkEmp.Role = employee.Role;
            //checkEmp.EmployeeCode = employee.EmployeeCode;
            //checkEmp.Unit = employee.Unit;
            //checkEmp.DepartmentId = employee.DepartmentId;
            //checkEmp.EmployeeMobile = employee.EmployeeMobile;
            //checkEmp.Email = employee.Email;
            //checkEmp.EmployeeExtension = employee.EmployeeExtension;
            //if (await _context.SaveChangesAsync() > 0)
            //{
            //    return employee;
            //}
            //else
            //{
            //    throw new Exception("Employee not updated. Failed :(");
            //}
            var hasher = new PasswordHasher<MasterEmployee>();
            var hashedPswd = hasher.HashPassword(null, employee.Password);
            employee.Password = hashedPswd;
            employee.LastPasswordChanged = DateTime.Now;
            var affectedRows = await _context.Database.ExecuteSqlRawAsync("EXEC sp_UpdateEmployee @Id={0}, @EmployeeName={1}, @Password={2}, @Role={3}, @EmployeeCode={4}, @Unit={5}, @DepartmentId={6}, @EmployeeMobile={7}, @Email={8}, @EmployeeExtension={9}",
                id, employee.EmployeeName, employee.Password, employee.Role, employee.EmployeeCode, employee.Unit, employee.DepartmentId, employee.EmployeeMobile, employee.Email, employee.EmployeeExtension);


            if (affectedRows > 0)
            {
                return employee;
            }
            throw new Exception("Failed to update employee");
        }

        public async Task<IEnumerable<MasterEmployee>> GetEmployeesByDepartmentIdAndEmployeeDetails(int departmentId, string inpQuery)
        {
            //var checkEmployees = await _context.MasterEmployees.Where(me => me.DepartmentId == departmentId).ToListAsync();
            //if(checkEmployees == null)
            //{
            //    throw new NotFoundException($"Employees not found. Probably department id is incorrect.");
            //}
            //var foundEmployees = await _context.MasterEmployees.Where(me => (me.DepartmentId == departmentId) && (me.EmployeeName.Contains(inpQuery) || me.EmployeeCode.Contains(inpQuery))).ToListAsync();
            //return foundEmployees;

            var checkEmployees = await _context.MasterEmployees.FromSqlRaw("EXEC sp_GetEmployeesByDepartmentAndQuery @DepartmentId={0}, @inpQuery={1}", departmentId, inpQuery).ToListAsync();
            if (checkEmployees == null)
            {
                throw new NotFoundException($"No employees found for department ID: {departmentId}");
            }
            return checkEmployees;
        }
    }
}
