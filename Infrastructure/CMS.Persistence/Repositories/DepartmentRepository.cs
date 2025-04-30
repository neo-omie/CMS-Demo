using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Exceptions;
using CMS.Application.Features.ApprovalMatrixContract.Commands;
using CMS.Application.Features.ApprovalMatrixMOU.Commands.UpdateApprovalMatrixMOU;
using CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOU;
using CMS.Application.Features.Departments.Queries.GetAllDepartments;
using CMS.Application.Features.EscalationMatrixMouMaster.Commands.UpdateEscalationMatrixMou;
using CMS.Application.Features.MasterEscalationMatrixContracts.Command;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CMS.Persistence.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        readonly CMSDbContext _context;
        public DepartmentRepository(CMSDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<GetAllDepartmentsDto>> GetAllDepartments(int pageNumber, int pageSize)
        {
            int totalRecords = await _context.Departments.CountAsync();
            return _context.Departments.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Select(a => new GetAllDepartmentsDto
                {
                    DepartmentId = a.DepartmentId,
                    DepartmentName = a.DepartmentName,
                    TotalRecords = totalRecords
                });
        }

        public async Task<Department> GetDepartmentById(int id)
        {
            var foundDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
            if(foundDepartment == null)
            {
                throw new NotFoundException($"Department with ID {id} not found.");
            }
            return foundDepartment;
        }
        public async Task<Department> AddNewDepartment(string departmentName)
        {
            var checkDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentName == departmentName);
            if(checkDepartment != null)
            {
                throw new Exception($"{departmentName} Department already exists.");
            }
            var newDepartment = new Department { DepartmentName = departmentName };
            await _context.Departments.AddAsync(newDepartment);
            if (await _context.SaveChangesAsync() > 0)
            {
                return newDepartment;
            }
            else
            {
                throw new Exception($"For some reasons, {departmentName} not added");
            }
        }

        public async Task<bool> DeleteDepartment(int id)
        {
            var checkDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
            if (checkDepartment == null)
            {
                throw new NotFoundException($"Department with {id} not found.");
            }
            _context.Departments.Remove(checkDepartment);
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            else
            {
                throw new Exception($"For some reasons, department with id {id} has not been deleted.");
            }
        }


        public async Task<bool> UpdateApprovalMatrixMOU(int id, string departmentName)
        {
            var checkDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
            if (checkDepartment == null)
            {
                throw new NotFoundException($"Department with {id} not found.");
            }
            checkDepartment.DepartmentName = departmentName;
            _context.Departments.Update(checkDepartment);
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            else
            {
                throw new Exception($"For some reasons, department with id {id} has not been updated.");
            }
        }
        // Adding Escalators and Approvers for Contracts and MOUs after creating a new department
        public async Task<MasterApprovalMatrixContract> AddContractApprovers(int id, UpdateApprovalMatrixContractDto addApprovers)
        {
            var checkDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
            if (checkDepartment == null)
            {
                throw new NotFoundException($"Department with {id} not found.");
            }
            var checkDepartmentInMatrix = await _context.MasterApprovalMatrixContracts.FirstOrDefaultAsync(d => d.DepartmentId == id);
            if (checkDepartmentInMatrix != null)
            {
                throw new Exception($"Contract Approvers for Department with {id} already exists");
            }
            var newDepartmentApprovers = new MasterApprovalMatrixContract
            {
                DepartmentId = id,
                ApproverId1 = addApprovers.ApproverId1,
                ApproverId2 = addApprovers.ApproverId2,
                ApproverId3 = addApprovers.ApproverId3,
                NumberOfDays = addApprovers.NumberOfDays
            };
            await _context.MasterApprovalMatrixContracts.AddAsync(newDepartmentApprovers);
            if(await _context.SaveChangesAsync() > 0)
            {
                return newDepartmentApprovers;
            }
            else
            {
                throw new Exception($"For some reasons, contract approvers for department id {id} has not been added");
            }
        }

        public async Task<MasterApprovalMatrixMOU> AddMOUApprovers(int id, UpdateApprovalMatrixMOUDto addApprovers)
        {
            var checkDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
            if (checkDepartment == null)
            {
                throw new NotFoundException($"Department with {id} not found.");
            }
            var checkDepartmentInMatrix = await _context.MasterApprovalMatrixMOUs.FirstOrDefaultAsync(d => d.DepartmentId == id);
            if (checkDepartmentInMatrix != null)
            {
                throw new Exception($"MOU Approvers for Department with {id} already exists");
            }
            var newDepartmentApprovers = new MasterApprovalMatrixMOU
            {
                DepartmentId = id,
                ApproverId1 = addApprovers.ApproverId1,
                ApproverId2 = addApprovers.ApproverId2,
                ApproverId3 = addApprovers.ApproverId3,
                NumberOfDays = addApprovers.NumberOfDays
            };
            await _context.MasterApprovalMatrixMOUs.AddAsync(newDepartmentApprovers);
            if (await _context.SaveChangesAsync() > 0)
            {
                return newDepartmentApprovers;
            }
            else
            {
                throw new Exception($"For some reasons, MOU approvers for department id {id} has not been added");
            }
        }

        public async Task<MasterEscalationMatrixContract> AddContractEscalators(int id, UpdateEscalationMatrixContractDto addEscalators)
        {
            var checkDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
            if (checkDepartment == null)
            {
                throw new NotFoundException($"Department with {id} not found.");
            }
            var checkDepartmentInMatrix = await _context.MasterEscalationMatrixContracts.FirstOrDefaultAsync(d => d.DepartmentId == id);
            if (checkDepartmentInMatrix != null)
            {
                throw new Exception($"Contract Escalators for Department with {id} already exists");
            }
            var newDepartmentEscalators = new MasterEscalationMatrixContract
            {
                DepartmentId = id,
                EscalationId1 = addEscalators.EscalationId1,
                EscalationId2 = addEscalators.EscalationId2,
                EscalationId3 = addEscalators.EscalationId3,
                TriggerDaysEscalation1 = addEscalators.TriggerDaysEscalation1,
                TriggerDaysEscalation2 = addEscalators.TriggerDaysEscalation2,
                TriggerDaysEscalation3 = addEscalators.TriggerDaysEscalation3
            };
            await _context.MasterEscalationMatrixContracts.AddAsync(newDepartmentEscalators);
            if (await _context.SaveChangesAsync() > 0)
            {
                return newDepartmentEscalators;
            }
            else
            {
                throw new Exception($"For some reasons, contract escalators for department id {id} has not been added");
            }
        }
        public async Task<MasterEscalationMatrixMou> AddMouEscalators(int id, UpdateEscalationMatrixMouDto addEscalators)
        {
            var checkDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
            if (checkDepartment == null)
            {
                throw new NotFoundException($"Department with {id} not found.");
            }
            var checkDepartmentInMatrix = await _context.MasterEscalationMatrixMous.FirstOrDefaultAsync(d => d.DepartmentId == id);
            if (checkDepartmentInMatrix != null)
            {
                throw new Exception($"Mou Escalators for Department with {id} already exists");
            }
            var newDepartmentEscalators = new MasterEscalationMatrixMou
            {
                DepartmentId = id,
                EscalationId1 = addEscalators.EscalationId1,
                EscalationId2 = addEscalators.EscalationId2,
                EscalationId3 = addEscalators.EscalationId3,
                TriggerDaysEscalation1 = addEscalators.TriggerDaysEscalation1,
                TriggerDaysEscalation2 = addEscalators.TriggerDaysEscalation2,
                TriggerDaysEscalation3 = addEscalators.TriggerDaysEscalation3
            };
            await _context.MasterEscalationMatrixMous.AddAsync(newDepartmentEscalators);
            if (await _context.SaveChangesAsync() > 0)
            {
                return newDepartmentEscalators;
            }
            else
            {
                throw new Exception($"For some reasons, mou escalators for department id {id} has not been added");
            }
        }
    }
}
