using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CMS.Application.Features.Departments.Commands.UpdateDepartment
{
    public record UpdateDepartmentCommand(int id, string departmentName) : IRequest<bool>;
}
