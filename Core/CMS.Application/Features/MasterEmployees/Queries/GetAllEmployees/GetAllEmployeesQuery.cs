using CMS.Application.DTOs;
using CMS.Application.Features.MasterEmployees.EmployeeDtos;
using MediatR;

namespace CMS.Application.Features.MasterEmployees.Queries.GetAllEmployees
{
    public class GetAllEmployeesQuery : IRequest<PaginationDto<GetAllEmployeeDto>>
    {
        public string Unit { get; set; }
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
