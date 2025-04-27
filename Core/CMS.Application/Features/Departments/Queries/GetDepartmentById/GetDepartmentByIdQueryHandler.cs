using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Departments.Queries.GetDepartmentById
{
    public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, Department>
    {
        readonly IDepartmentRepository _departmentRepository;
        public GetDepartmentByIdQueryHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public async Task<Department> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _departmentRepository.GetDepartmentById(request.id);
        }
    }
}
