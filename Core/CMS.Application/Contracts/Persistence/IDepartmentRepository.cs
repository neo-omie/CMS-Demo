using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.ApprovalMatrixContract.Commands;
using CMS.Application.Features.ApprovalMatrixMOU.Commands.UpdateApprovalMatrixMOU;
using CMS.Application.Features.Departments.Queries.GetAllDepartments;
using CMS.Application.Features.EscalationMatrixMouMaster.Commands.UpdateEscalationMatrixMou;
using CMS.Application.Features.MasterEscalationMatrixContracts.Command;
using CMS.Domain.Entities;

namespace CMS.Application.Contracts.Persistence
{
    public interface IDepartmentRepository
    {
        public Task<IEnumerable<GetAllDepartmentsDto>> GetAllDepartments(int pageNumber, int pageSize);
        public Task<Department> GetDepartmentById(int id);
        public Task<Department> AddNewDepartment(string departmentName);
        public Task<bool> DeleteDepartment(int id);
        public Task<bool> UpdateApprovalMatrixMOU(int id, string departmentName);

        public Task<MasterApprovalMatrixContract> AddContractApprovers(int id, UpdateApprovalMatrixContractDto addApprovers);
        public Task<MasterApprovalMatrixMOU> AddMOUApprovers(int id, UpdateApprovalMatrixMOUDto addApprovers);
        public Task<MasterEscalationMatrixContract> AddContractEscalators(int id, UpdateEscalationMatrixContractDto addEscalators);
        public Task<MasterEscalationMatrixMou> AddMouEscalators(int id, UpdateEscalationMatrixMouDto addEscalators);
    }
}
