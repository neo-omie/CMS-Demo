


using CMS.Domain.Entities;

namespace CMS.Application.Contracts.Persistence
{
    public interface IMasterEmployeeRepository
    {
        Task<IEnumerable<MasterEmployee>> GetAllEmployeesAsync(string unit, string searchTerm, int pageNumber, int pageSize);
        Task<MasterEmployee> GetEmployeeByIdAsync(string id);
        Task<MasterEmployee> AddEmployeeAsync(MasterEmployee employee);
        Task<MasterEmployee> UpdateEmployeeAsync(MasterEmployee employee);
        Task<bool> DeleteEmployeeAsync(MasterEmployee employee);

    }
}
