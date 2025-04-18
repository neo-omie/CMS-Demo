using CMS.Application.Features.MasterEmployees.Commands.AddEmployee;
using CMS.Application.Features.MasterEmployees.Commands.DeleteEmployee;
using CMS.Application.Features.MasterEmployees.Commands.UpdateEmployee;
using CMS.Application.Features.MasterEmployees.EmployeeDtos;
using CMS.Application.Features.MasterEmployees.Queries.GetAllEmployees;
using CMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MasterEmployee>>> GetEmployees(
        [FromQuery] string unit,
        [FromQuery] string searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetAllEmployeesQuery
        (
            unit, searchTerm, pageNumber, pageSize
        );

        return Ok(await _mediator.Send(query));
    }

    [HttpPost]
    public async Task<ActionResult<MasterEmployee>> AddEmployee([FromBody] AddEmployeeDto employee)
    {
        var command = new AddEmployeeCommand(employee);
        return Ok(await _mediator.Send(command));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MasterEmployee>> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto employee)
    {
        var command = new UpdateEmployeeCommand(id,employee);
        return await _mediator.Send(command);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var command = new DeleteEmployeeCommand(id);
        var checkDelete= await _mediator.Send(command);
        if (checkDelete)
            return Ok("Successfully Deleted!!!");
        return BadRequest();
    }
}