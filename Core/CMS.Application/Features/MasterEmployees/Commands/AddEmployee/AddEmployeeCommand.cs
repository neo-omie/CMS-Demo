using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.MasterEmployees.EmployeeDtos;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterEmployees.Commands.AddEmployee
{
    public record AddEmployeeCommand(AddEmployeeDto EmpDTO):IRequest<MasterEmployee>;
}
