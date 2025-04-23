using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.MasterEmployees.EmployeeDtos;
using MediatR;

namespace CMS.Application.Features.MasterEmployees.Queries.GetEmployeesByDepartmentIdAndEmployeeDetails
{
    public record GetEmployeeByDepartmentIdAndEmployeeDetailsQuery(int departmentId, string inpQuery) : IRequest<IEnumerable<GetEmployeesByDepartmentIdAndEmpDetailsDto>>;
}
