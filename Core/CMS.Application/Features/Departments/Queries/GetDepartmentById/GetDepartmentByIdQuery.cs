using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Departments.Queries.GetDepartmentById
{
    public record GetDepartmentByIdQuery(int id) : IRequest<Department>;
}
