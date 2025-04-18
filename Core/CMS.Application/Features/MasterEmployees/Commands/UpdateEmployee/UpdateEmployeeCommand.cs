using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.MasterEmployees.EmployeeDtos;
using MediatR;

namespace CMS.Application.Features.MasterEmployees.Commands.UpdateEmployee
{
    public record UpdateEmployeeCommand(EmployeeDto empDTO):IRequest<UpdateEmployeeDto>;
}
