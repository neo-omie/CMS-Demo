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
    public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, IEnumerable<MasterEmployee>>
    {
        private readonly IMasterEmployeeRepository _masterEmployeeRepository;

        public GetAllEmployeesQueryHandler(IMasterEmployeeRepository masterEmployeeRepository)
        {
            _masterEmployeeRepository = masterEmployeeRepository;
        }

        public async Task<IEnumerable<MasterEmployee>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            return await _masterEmployeeRepository.GetAllEmployeesAsync(request.unit, request.searchTerm, request.pageNumber, request.pageSize);
        }
    }
}
