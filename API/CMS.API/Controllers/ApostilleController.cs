using CMS.Application.Features.MasterApostilles.ApostilleDtos;
using CMS.Application.Features.MasterApostilles.Commands.AddApostille;
using CMS.Application.Features.MasterApostilles.Commands.DeleteApostille;
using CMS.Application.Features.MasterApostilles.Commands.UpdateApostille;
using CMS.Application.Features.MasterApostilles.Queries.GetAllApostille;
using CMS.Application.Features.MasterApostilles.Queries.GetApostilleById;
using CMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApostilleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApostilleController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("{pageNumber}/{pageSize}")]
        public async Task<ActionResult<IEnumerable<GetAllApostilleDto>>> GetAllApostilleAsync(
        [FromRoute] int pageNumber,
        [FromRoute] int pageSize,
        [FromQuery] string? searchTerm)
        {
            var query= new GetAllApostilleQuery(pageNumber, pageSize, searchTerm);
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MasterApostille>> GetApostilleByIdAsync(int id)
        {
            var query = new GetApostilleByIdQuery(id);
            return Ok(await _mediator.Send(query));
        }

        [HttpPost("{empCode}")]
        public async Task<ActionResult<MasterApostille>> AddApostilleAsync([FromBody] AddApostilleDto apostille, [FromRoute] string empCode)
        {
            var command = new AddApostilleCommand(apostille,empCode);
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("{id}/{empCode}")]
        public async Task<ActionResult<MasterApostille>> UpdateApostilleAsync(int id, [FromBody] UpdateApostilleDto apostille, [FromRoute] string empCode)
        {
            var command= new UpdateApostilleCommand(id, apostille,empCode);
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("{id}/{empCode}")]
        public async Task<ActionResult<bool>> DeleteMasterApostilleAsync(int id, [FromRoute] string empCode)
        {
            var command= new DeleteApostilleCommand(id,empCode);
            var result = await _mediator.Send(command);
            if(result)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
