using CMS.Application.Features.Contracts;
using CMS.Application.Features.Contracts.Commands.CreateNewContract;
using CMS.Application.Features.Contracts.Commands.EditContract;
using CMS.Application.Features.Contracts.Queries.GetAllContracts;
using CMS.Application.Features.Contracts.Queries.GetContractById;
using CMS.Application.Features.ContractTypeMaster.Command.DeleteContract;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        readonly IMediator _mediator;
        readonly ILogger<ContractController> _logger;
        public ContractController(IMediator mediator, ILogger<ContractController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        //Get all Contracts
        [HttpGet]
        public async Task<IActionResult> GetAllContracts(int pageNumber, int pageSize)
        {
            var allContracts = await _mediator.Send(new GetAllContractsQuery(pageNumber, pageSize));
            return Ok(allContracts);
        }
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetContractById([FromRoute]int id)
        {
            var foundContract = await _mediator.Send(new GetContractByIdQuery(id));
            return Ok(foundContract);
        }
        [HttpPost]
        public async Task<IActionResult> AddContract(ContractDTO cont)
        {
            var addedContract = await _mediator.Send(new CreateNewContractCommand(cont));
            return Ok(addedContract);
        }
        [Route("{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateContract([FromRoute]int id, [FromBody]ContractDTO cont)
        {
            var editedContract = await _mediator.Send(new EditContractCommand(id, cont));
            return Ok(editedContract); // bool
        }
        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteContract([FromRoute] int id)
        {
            var deletedContract = await _mediator.Send(new DeleteContractCommand(id));
            return Ok(deletedContract); // bool
        }
    }
}
