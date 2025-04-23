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

namespace CMS.Application.Features.MasterEmployees.Queries.GetEmployeesByDepartmentIdAndEmployeeDetails
{
    public class GetEmployeeByDepartmentIdAndEmployeeDetailsQueryHandler : IRequestHandler<GetEmployeeByDepartmentIdAndEmployeeDetailsQuery, IEnumerable<GetEmployeesByDepartmentIdAndEmpDetailsDto>>
    {
        readonly IMasterEmployeeRepository _masterEmployeeRepository;
        readonly IMapper _mapper;
        public GetEmployeeByDepartmentIdAndEmployeeDetailsQueryHandler(IMasterEmployeeRepository masterEmployeeRepository, IMapper mapper)
        {
            _masterEmployeeRepository = masterEmployeeRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetEmployeesByDepartmentIdAndEmpDetailsDto>> Handle(GetEmployeeByDepartmentIdAndEmployeeDetailsQuery request, CancellationToken cancellationToken)
        {
            var foundEmployees =  await _masterEmployeeRepository.GetEmployeesByDepartmentIdAndEmployeeDetails(request.departmentId, request.inpQuery);
            var mappedEmployees = _mapper.Map<IEnumerable<GetEmployeesByDepartmentIdAndEmpDetailsDto>>(foundEmployees);
            return mappedEmployees;
        }
    }
}
