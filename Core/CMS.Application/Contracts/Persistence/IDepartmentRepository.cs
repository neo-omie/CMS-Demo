using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Application.Contracts.Persistence
{
    public interface IDepartmentRepository
    {
        public Task<IEnumerable<Department>> GetAllDepartments(int pageNumber, int pageSize);
        public Task<Department> GetDepartmentById(int id);
        public Task<Department> AddNewDepartment(string departmentName);
        public Task<bool> DeleteDepartment(int id);
        public Task<bool> UpdateApprovalMatrixMOU(int id, string departmentName);
    }
}
