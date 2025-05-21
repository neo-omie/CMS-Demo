using CMS.Application.Features.AddendumContract.AddendumContractDto;
using CMS.Application.Features.AddendumContracts.Commands.AddAddendumContract;
using CMS.Application.Features.AddendumContracts.Commands.ApproveRejectAddendum;
using CMS.Application.Features.AddendumContracts.Commands.DeleteAddendumCotract;
using CMS.Application.Features.AddendumContracts.Queries.GetAddendumContractById;
using CMS.Application.Features.AddendumContracts.Queries.GetAllAddendumByContractId;
using CMS.Application.Features.AddendumContracts.Queries.GetAllAddendumContracts;
using CMS.Application.Features.Contracts.Commands.ApproveRejectContract;
using CMS.Domain.Constants;
using CMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddendumContractController : ControllerBase
    {
        private readonly IMediator _mediator;
        readonly ILogger<ContractController> _logger;
        public AddendumContractController(IMediator mediator, ILogger<ContractController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Route("{pageNumber}/{pageSize}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddendumContract>>> GetAllAddendums(
            [FromRoute] int pageNumber,
            [FromRoute] int pageSize,
            [FromQuery]DateTime? searchTerm
        )
        {
            var query= new GetAllAddendumContractsQuery(pageNumber, pageSize, searchTerm);
            return Ok(await _mediator.Send(query));
        }

        [Route("{pageNumber}/{pageSize}/{contractId}")]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<AddendumContract>>> GetAllAddendums(
            [FromRoute] int pageNumber,
            [FromRoute] int pageSize,
            [FromRoute] int contractId
        )
        {
            var query = new GetAllAddendumByContractIdQuery(pageNumber, pageSize, contractId);
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AddendumContract>> GetAddedumById(int id)
        {
            var query= new GetAddedumContractByIdQuery(id);
            return Ok(await _mediator.Send(query));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddedum(int id)
        {
            var command = new DeleteAddendumContractCommand(id);
            var checkDelete = await _mediator.Send(command);
            if (checkDelete)
                return Ok(checkDelete);
            return NotFound();
        }


        [HttpPost("{id}")]
        public async Task<ActionResult<AddendumContract>> AddAddendum(int id, [FromBody] AddAddendumContractDto addendum)
        {
            var command= new AddAddendumContractCommand(id, addendum);
            return Ok(await _mediator.Send(command));
        }

        [Route("{addendumId}/approveRejectAddendum/{empCode}/{addendumStatus}")] 
        [HttpPost]
        public async Task<IActionResult> AddendumApprove(int contractId, ContractStatus addendumStatus, int addendumId, string empCode)
        {
            _logger.LogInformation("AddendumApprove method initiated");
            AddendumContract contract = null;
            if ((ContractStatus)addendumStatus == ContractStatus.Active)
            {
                contract = await _mediator.Send(new ApproveRejectAddendumCommand(contractId, ContractStatus.Active, addendumId, empCode));
            }
            else if ((ContractStatus)addendumStatus == ContractStatus.Rejected)
            {
                contract = await _mediator.Send(new ApproveRejectAddendumCommand(contractId, ContractStatus.Rejected, addendumId, empCode));
            }
            else
            {
                return BadRequest("Wrong contract status number");
            }
            _logger.LogInformation("AddendumApprove method performed");
            if (contract != null)
                return Ok(true);
            return Ok(false);
        }
    }
}
