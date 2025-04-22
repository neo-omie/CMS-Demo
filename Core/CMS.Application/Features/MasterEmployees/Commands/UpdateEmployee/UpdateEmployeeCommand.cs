using CMS.Application.Features.MasterEmployees.EmployeeDtos;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterEmployees.Commands.UpdateEmployee
{
    public record UpdateEmployeeCommand(int id,UpdateEmployeeDto EmpDTO):IRequest<MasterEmployee>;
}
