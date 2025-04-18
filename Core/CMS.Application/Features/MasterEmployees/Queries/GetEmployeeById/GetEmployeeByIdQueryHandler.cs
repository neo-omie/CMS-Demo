using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterEmployees.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, MasterEmployee>
    {
        private readonly IMasterEmployeeRepository _masterEmployeeRepository;

        public GetEmployeeByIdQueryHandler(IMasterEmployeeRepository masterEmployeeRepository)
        {
            _masterEmployeeRepository = masterEmployeeRepository;
        }
        public async Task<MasterEmployee> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            return await _masterEmployeeRepository.GetEmployeeByIdAsync(request.id);
        }
    }
}
