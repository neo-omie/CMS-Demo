using MediatR;
using Microsoft.AspNetCore.Mvc;
using CMS.Application.Features.EscalationMatrixMouMaster.Queries.GetAllEscalationMatrixMou;
using CMS.Application.Features.EscalationMatrixMouMaster.Queries.GetEscalationMatrixMoutById;
using CMS.Application.Features.EscalationMatrixMouMaster.Commands.UpdateEscalationMatrixMou;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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


        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateMatrixMou(int id, [FromBody] UpdateEscalationMatrixMouDto updateDto)
        {
            await _mediator.Send(new UpdateEscalationMatrixMouCommand(id, updateDto));
            return Ok(new { Message = "Updated successfully" });
        }
    }
}
