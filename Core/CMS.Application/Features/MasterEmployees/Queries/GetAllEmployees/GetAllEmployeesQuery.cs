using CMS.Application.DTOs;
using CMS.Application.Features.MasterEmployees.EmployeeDtos;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterEmployees.Queries.GetAllEmployees
{
    public record GetAllEmployeesQuery(string unit, string searchTerm, int pageNumber, int pageSize) : IRequest<IEnumerable<MasterEmployee>>;
}
