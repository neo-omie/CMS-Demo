using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Departments.Queries.SearchDepartment
{
    public class SearchDepartmentQueryHandler : IRequestHandler<SearchDepartmentQuery, IEnumerable<Department>>
    {
        readonly IDepartmentRepository _departmentRepository;
        public SearchDepartmentQueryHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public async Task<IEnumerable<Department>> Handle(SearchDepartmentQuery request, CancellationToken cancellationToken)
        {
            return await _departmentRepository.SearchDepartment(request.searchQuery);
        }
    }
}
