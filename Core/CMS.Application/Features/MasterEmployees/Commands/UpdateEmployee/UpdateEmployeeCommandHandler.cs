using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.MasterEmployees.EmployeeDtos;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterEmployees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, MasterEmployee>
    {
        private readonly IMasterEmployeeRepository _masterEmployeeRepository;
        private readonly IMapper _mapper;

        public UpdateEmployeeCommandHandler(IMasterEmployeeRepository masterEmployeeRepository, IMapper mapper)
        {
            _masterEmployeeRepository = masterEmployeeRepository;
            _mapper = mapper;
        }
        public async Task<MasterEmployee> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = _mapper.Map<MasterEmployee>(request.EmpDTO);
            return await _masterEmployeeRepository.UpdateEmployeeAsync(request.id, employee);
        }
    }
}
