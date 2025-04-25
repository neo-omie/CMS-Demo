using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Departments.Commands.AddDepartment
{
    public class AddDepartmentCommandHandler : IRequestHandler<AddDepartmentCommand, Department>
    {
        readonly IDepartmentRepository _departmentRepository;
        public AddDepartmentCommandHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public async Task<Department> Handle(AddDepartmentCommand request, CancellationToken cancellationToken)
        {
            return await _departmentRepository.AddNewDepartment(request.departmentName);
        }
    }
}
