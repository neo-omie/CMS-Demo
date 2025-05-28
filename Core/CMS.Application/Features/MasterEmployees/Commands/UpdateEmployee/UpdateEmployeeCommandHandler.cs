using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json.Serialization;
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
        //private readonly IAuditLogService _auditLogService;

        public UpdateEmployeeCommandHandler(IMasterEmployeeRepository masterEmployeeRepository, IMapper mapper)
        {
            _masterEmployeeRepository = masterEmployeeRepository;
            _mapper = mapper;
            //_auditLogService = auditLogService;
        }
        public async Task<MasterEmployee> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = _mapper.Map<MasterEmployee>(request.EmpDTO);

            //var existingEmployee = await _masterEmployeeRepository.GetEmployeeByIdAsync(request.id);
            //if (existingEmployee == null)
            //{
            //    throw new Exception("Employee with this id not found");
            //}

            //var OldValues = JsonConverter.SerializeObject(existingEmployee);
            //var NewValues = JsonConverter.SerializeObject(employee);

            //await _auditLogService.LogActionAsync(
            //   action: "Update",
            //   tableName: "MasterEmployees",
            //   recordId: request.id,
            //   oldValue: oldValues,
            //   newValue: newValues,
            //   loggedBy: request.EmpDTO.LoggedBy // Assuming LoggedBy is part of the DTO
            //);
            return await _masterEmployeeRepository.UpdateEmployeeAsync(request.id, employee,request.empCode);

        }
    }
}
