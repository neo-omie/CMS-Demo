using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterEmployees.Queries.GetAllEmployees
{
    public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, object>
    {
        private readonly IMasterEmployeeRepository _masterEmployeeRepository;

        public GetAllEmployeesQueryHandler(IMasterEmployeeRepository masterEmployeeRepository)
        {
            _masterEmployeeRepository = masterEmployeeRepository;
        }

        public async Task<object> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            var result = await _masterEmployeeRepository.GetAllEmployeesAsync(request.pageNumber, request.pageSize, request?.unit, request?.searchTerm);
            return new
            {
                data = result.Data,
                totalCount = result.TotalCount
            };
        }
    }
}
