using CMS.Application.Features.ApprovalMatrixContract.Commands;
using CMS.Application.Features.ApprovalMatrixMOU.Commands.UpdateApprovalMatrixMOU;
using CMS.Application.Features.Departments.Commands.AddContractApprovers;
using CMS.Application.Features.Departments.Commands.AddContractEscalators;
using CMS.Application.Features.Departments.Commands.AddDepartment;
using CMS.Application.Features.Departments.Commands.AddMOUApprovers;
using CMS.Application.Features.Departments.Commands.AddMouEscalators;
using CMS.Application.Features.Departments.Commands.DeleteDepartment;
using CMS.Application.Features.Departments.Commands.UpdateDepartment;
using CMS.Application.Features.Departments.Queries.GetAllDepartments;
using CMS.Application.Features.Departments.Queries.GetDepartmentById;
using CMS.Application.Features.Departments.Queries.SearchDepartment;
using CMS.Application.Features.EscalationMatrixMouMaster.Commands.UpdateEscalationMatrixMou;
using CMS.Application.Features.MasterEscalationMatrixContracts.Command;
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

        [HttpGet("Search")]
        public async Task<IActionResult> SearchDepartment(string searchQuery)
        {
            var departments = await _mediator.Send(new SearchDepartmentQuery(searchQuery));
            return Ok(departments);
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

        [HttpPost("AddContractApprovers")]
        public async Task<IActionResult> AddContractApprovers([FromQuery] int id, [FromBody] UpdateApprovalMatrixContractDto addApprovers)
        {
            var addedApprovers = await _mediator.Send(new AddContractApproversCommand(id, addApprovers));
            return Ok(addedApprovers);
        }

        [HttpPost("AddMOUApprovers")]
        public async Task<IActionResult> AddMOUApprovers([FromQuery] int id, [FromBody] UpdateApprovalMatrixMOUDto addApprovers)
        {
            var addedApprovers = await _mediator.Send(new AddMOUApproversCommand(id, addApprovers));
            return Ok(addedApprovers);
        }

        [HttpPost("AddContractEscalators")]
        public async Task<IActionResult> AddContractEscalators([FromQuery] int id, [FromBody] UpdateEscalationMatrixContractDto addEscalators)
        {
            var addedEscalators = await _mediator.Send(new AddContractEscalatorsCommand(id, addEscalators));
            return Ok(addedEscalators);
        }
        [HttpPost("AddMouEscalators")]
        public async Task<IActionResult> AddMouEscalators([FromQuery] int id, [FromBody] UpdateEscalationMatrixMouDto addEscalators)
        {
            var addedEscalators = await _mediator.Send(new AddMouEscalatorsCommand(id, addEscalators));
            return Ok(addedEscalators);
        }
    }
}
