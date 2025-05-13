using CMS.Application.Features.AddendumContract.AddendumContractDto;
using CMS.Application.Features.AddendumContracts.Commands.AddAddendumContract;
using CMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddendumContractController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AddendumContractController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AddendumContract>> AddAddendum(int id, [FromBody] AddAddendumContractDto addendum)
        {
            var command= new AddAddendumContractCommand(id, addendum);
            return Ok(await _mediator.Send(command));
        }

    }
}
