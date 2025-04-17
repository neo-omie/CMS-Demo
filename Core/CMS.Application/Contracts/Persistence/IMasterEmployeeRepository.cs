


using CMS.Domain.Entities;

namespace CMS.Application.Contracts.Persistence
{
    public interface IMasterEmployeeRepository
    {
        Task<IEnumerable<MasterEmployee>> GetAllEmployeesAsync(string unit, string searchTerm, int pageNumber, int pageSize);
        Task<MasterEmployee> GetEmployeeByIdAsync(string id);
        Task AddEmployeeAsync(MasterEmployee employee);
        Task UpdateEmployeeAsync(MasterEmployee employee);
        Task DeleteEmployeeAsync(MasterEmployee employee);

    }
}
