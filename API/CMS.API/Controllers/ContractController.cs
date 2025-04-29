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
            _logger.LogInformation("GetAllContracts method initiated");
            var allContracts = await _mediator.Send(new GetAllContractsQuery(pageNumber, pageSize));
            _logger.LogInformation("GetAllContracts method performed");
            return Ok(allContracts);
        }
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetContractById([FromRoute]int id)
        {
            _logger.LogInformation("GetContractById method initiated");
            var foundContract = await _mediator.Send(new GetContractByIdQuery(id));
            _logger.LogInformation("GetContractById method performed");
            return Ok(foundContract);
        }
        [HttpPost]
        public async Task<IActionResult> AddContract(ContractDTO cont)
        {
            _logger.LogInformation("AddContract method initiated");
            var addedContract = await _mediator.Send(new CreateNewContractCommand(cont));
            _logger.LogInformation("AddContract method performed");
            return Ok(addedContract);
        }
        [Route("{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateContract([FromRoute]int id, [FromBody]ContractDTO cont)
        {
            _logger.LogInformation("UpdateContract method initiated");
            var editedContract = await _mediator.Send(new EditContractCommand(id, cont));
            _logger.LogInformation("UpdateContract method performed");
            return Ok(editedContract); // bool
        }
        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteContract([FromRoute] int id)
        {
            _logger.LogInformation("DeleteContract method initiated");
            var deletedContract = await _mediator.Send(new DeleteContractCommand(id));
            _logger.LogInformation("DeleteContract method performed");
            return Ok(deletedContract); // bool
        }
    }
}
