using CMS.Application.Features.Departments.Commands.AddDepartment;
using CMS.Application.Features.Departments.Commands.DeleteDepartment;
using CMS.Application.Features.Departments.Commands.UpdateDepartment;
using CMS.Application.Features.Departments.Queries.GetAllDepartments;
using CMS.Application.Features.Departments.Queries.GetDepartmentById;
using CMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        readonly IMediator _mediator;
        public DepartmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments(int pageNumber, int pageSize)
        {
            var departments = await _mediator.Send(new GetAllDepartmentsQuery(pageNumber, pageSize));
            return Ok(departments);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetDepartmentById([FromRoute] int id)
        {
            var department = await _mediator.Send(new GetDepartmentByIdQuery(id));
            return Ok(department);
        }

        [HttpPost]
        public async Task<IActionResult> AddDepartment(string departmentName)
        {
            var newDepartment = await _mediator.Send(new AddDepartmentCommand(departmentName));
            return Ok(newDepartment);
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateDepartment([FromRoute] int id, string departmentName)
        {
            var checkUpdate = await _mediator.Send(new UpdateDepartmentCommand(id, departmentName));
            return Ok(checkUpdate);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteDepartment([FromRoute] int id)
        {
            var checkDelete = await _mediator.Send(new DeleteDepartmentCommand(id));
            return Ok(checkDelete);
        }
    }
}
