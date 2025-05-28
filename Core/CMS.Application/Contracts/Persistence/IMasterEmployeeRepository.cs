


using CMS.Application.Features.MasterEmployees.EmployeeDtos;
using CMS.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CMS.Application.Contracts.Persistence
{
    public interface IMasterEmployeeRepository
    {
        Task<(IEnumerable<MasterEmployee> Data, int TotalCount)> GetAllEmployeesAsync(int pageNumber, int pageSize, string unit, string searchTerm);
        Task<MasterEmployee> GetEmployeeByIdAsync(int id);
        Task<MasterEmployee> AddEmployeeAsync(MasterEmployee employee,string empCode);
        Task<MasterEmployee> UpdateEmployeeAsync(int id,MasterEmployee employee,string empCode);
        Task<bool> DeleteEmployeeAsync(int id,string empCode);
        Task<IEnumerable<MasterEmployee>> GetEmployeesByDepartmentIdAndEmployeeDetails(int departmentId, string inpQuery);

    }
}
