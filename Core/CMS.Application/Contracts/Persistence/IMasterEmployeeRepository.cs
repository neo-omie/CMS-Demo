


using CMS.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CMS.Application.Contracts.Persistence
{
    public interface IMasterEmployeeRepository
    {
        Task<(IEnumerable<MasterEmployee> Data, int TotalCount)> GetAllEmployeesAsync(int pageNumber, int pageSize, string unit, string searchTerm);
        Task<MasterEmployee> GetEmployeeByIdAsync(int id);
        Task<MasterEmployee> AddEmployeeAsync(MasterEmployee employee);
        Task<MasterEmployee> UpdateEmployeeAsync(int id,MasterEmployee employee);
        Task<bool> DeleteEmployeeAsync(int id);

        Task<IEnumerable<MasterEmployee>> GetEmployeesByDepartmentIdAndEmployeeDetails(int departmentId, string inpQuery);

    }
}
