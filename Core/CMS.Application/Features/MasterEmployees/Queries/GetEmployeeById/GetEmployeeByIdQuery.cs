using CMS.Application.Features.MasterEmployees.EmployeeDtos;
using MediatR;

namespace CMS.Application.Features.MasterEmployees.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQuery:IRequest<EmployeeDto>
    {
        public string Id { get; set; }
    }
}
