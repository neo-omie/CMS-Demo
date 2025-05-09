﻿using CMS.Application.Features.MasterEmployees.Commands.AddEmployee;
using CMS.Application.Features.MasterEmployees.Commands.DeleteEmployee;
using CMS.Application.Features.MasterEmployees.Commands.UpdateEmployee;
using CMS.Application.Features.MasterEmployees.EmployeeDtos;
using CMS.Application.Features.MasterEmployees.Queries.GetAllEmployees;
using CMS.Application.Features.MasterEmployees.Queries.GetEmployeeById;
using CMS.Application.Features.MasterEmployees.Queries.GetEmployeesByDepartmentIdAndEmployeeDetails;
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

    
    [HttpGet("{pageNumber}/{pageSize}")]
    public async Task<ActionResult<IEnumerable<MasterEmployee>>> GetAllEmployees(
        [FromRoute] int pageNumber,
        [FromRoute] int pageSize,
        [FromQuery] string? unit,
        [FromQuery] string? searchTerm
        )
    {
        var query = new GetAllEmployeesQuery
        (
             pageNumber, pageSize, unit, searchTerm
        );

        return Ok(await _mediator.Send(query));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetEmployeeByIDDto>> GetEmployeeById(int id)
    {
        var query = new GetEmployeeByIdQuery(id);
        return Ok(await _mediator.Send(query));
    }

    [HttpPost]
    public async Task<ActionResult<MasterEmployee>> AddEmployee([FromBody]AddEmployeeDto employee)
    {
        var command = new AddEmployeeCommand(employee);
        return Ok(await _mediator.Send(command));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MasterEmployee>> UpdateEmployee(int id, [FromBody]UpdateEmployeeDto employee)
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
            return Ok(checkDelete);
        return NotFound();
    }

    [HttpGet("search/{departmentId}/{inpQuery?}")]
    public async Task<IActionResult> GetEmployeesByDepartmentIdAndEmployeeDetails(int departmentId, string inpQuery = "")
    {
        if(string.IsNullOrEmpty(inpQuery) || string.IsNullOrWhiteSpace(inpQuery.Trim()))
        {
            return Ok(new List<GetEmployeesByDepartmentIdAndEmpDetailsDto>());
        }
        var query = new GetEmployeeByDepartmentIdAndEmployeeDetailsQuery(departmentId, inpQuery);
        var result = await _mediator.Send(query);
        return Ok(result);

    }
}