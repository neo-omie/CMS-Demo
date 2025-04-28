using CMS.Application.DTOs;
using CMS.Application.Features.MasterEmployees.EmployeeDtos;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterEmployees.Queries.GetAllEmployees
{
    public record GetAllEmployeesQuery(int pageNumber, int pageSize, string? unit, string? searchTerm) : IRequest<object>;
}
