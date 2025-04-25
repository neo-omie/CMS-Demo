using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Exceptions;
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
        public async Task<IEnumerable<Department>> GetAllDepartments(int pageNumber, int pageSize)
        {
            int totalRecords = await _context.Departments.CountAsync();
            return _context.Departments.Skip((pageNumber - 1) * pageSize).Take(pageSize);
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
    }
}
