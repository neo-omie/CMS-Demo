using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CMS.Application.Features.Departments.Commands.DeleteDepartment
{
    public record DeleteDepartmentCommand(int id) : IRequest<bool>;
}
