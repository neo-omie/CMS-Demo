using MediatR;
using Microsoft.AspNetCore.Mvc;
using CMS.Application.Features.EscalationMatrixMouMaster.Queries.GetAllEscalationMatrixMou;
using CMS.Application.Features.EscalationMatrixMouMaster.Queries.GetEscalationMatrixMoutById;
using CMS.Application.Features.EscalationMatrixMouMaster.Commands.UpdateEscalationMatrixMou;
using Microsoft.AspNetCore.Authorization;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin , Super_Admin , Management_User")]
    public class EscalationMatrixMouController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EscalationMatrixMouController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetAllmatrixMou([FromRoute] int pageNumber, [FromRoute] int pageSize)
        {
            var mous = await _mediator.Send(new GetAllEscalationMatrixMouQuery(pageNumber, pageSize));
            return Ok(mous);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatrixMouById(int id)
        {
            var contract = await _mediator.Send(new GetEscalationMatrixMouByIdQuery(id));
            return Ok(contract);
        }


        [HttpPost("empCode/${empCode}/{id}")]
        public async Task<IActionResult> UpdateMatrixMou(int id, [FromBody] UpdateEscalationMatrixMouDto updateDto, [FromRoute] string empCode)
        {
            if (updateDto.EscalationId1 == updateDto.EscalationId2 && updateDto.EscalationId1 != "NEO1" || 
                updateDto.EscalationId1 == updateDto.EscalationId3 && updateDto.EscalationId1 != "NEO1" || 
                updateDto.EscalationId2 == updateDto.EscalationId3 && updateDto.EscalationId2 != "NEO1")
            {
                throw new Exception("Escalators cannot be same");
            }
            await _mediator.Send(new UpdateEscalationMatrixMouCommand(id, updateDto, empCode));
            return Ok(new { Message = "Updated successfully" });
        }
    }
}
