using CMS.Application.Features.MasterEmployees.EmployeeDtos;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterEmployees.Queries.GetEmployeeById
{
    public record GetEmployeeByIdQuery(int id):IRequest<GetEmployeeByIDDto>;
}
