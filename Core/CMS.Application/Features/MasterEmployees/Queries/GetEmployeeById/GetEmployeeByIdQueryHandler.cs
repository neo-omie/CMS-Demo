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

namespace CMS.Application.Features.MasterEmployees.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, GetEmployeeByIDDto>
    {
        private readonly IMasterEmployeeRepository _masterEmployeeRepository;
        private readonly IMapper _mapper;
        private readonly IDepartmentRepository _departmentRepository;

        public GetEmployeeByIdQueryHandler(IMasterEmployeeRepository masterEmployeeRepository, IMapper mapper, IDepartmentRepository departmentRepository)
        {
            _mapper = mapper;
            _masterEmployeeRepository = masterEmployeeRepository;
            _departmentRepository = departmentRepository;
        }
        public async Task<GetEmployeeByIDDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var gotUser = await _masterEmployeeRepository.GetEmployeeByIdAsync(request.id);
            var mappedUser = _mapper.Map<GetEmployeeByIDDto>(gotUser);
            var gotDepartment = await _departmentRepository.GetDepartmentById(gotUser.DepartmentId);
            mappedUser.DepartmentName = gotDepartment.DepartmentName;
            return mappedUser;
        }
    }
}
