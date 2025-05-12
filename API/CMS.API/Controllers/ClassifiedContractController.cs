using CMS.Application.Features.ClassifiedContracts;
using CMS.Application.Features.ClassifiedContracts.Commands.CreateNewContract;
using CMS.Application.Features.ClassifiedContracts.Commands.EditClassifiedContract;
using CMS.Application.Features.ClassifiedContracts.Commands.RemoveClassifiedContract;
using CMS.Application.Features.ClassifiedContracts.Queries.GetAllClassifiedContracts;
using CMS.Application.Features.ClassifiedContracts.Queries.GetClassifiedContractById;
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
    public class ClassifiedContractController : ControllerBase
    {
        readonly IMediator _mediator;
        readonly ILogger<ContractController> _logger;
        public ClassifiedContractController(IMediator mediator, ILogger<ContractController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllClassifiedContracts(int pageNumber, int pageSize)
        {
            _logger.LogInformation("GetAllClassifiedContracts method initiated");
            var allContracts = await _mediator.Send(new GetAllClassifiedContractsQuery(pageNumber, pageSize));
            _logger.LogInformation("GetAllClassifiedClassifiedContracts method performed");
            return Ok(allContracts);
        }
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetClassifiedContractById([FromRoute]int id)
        {
            _logger.LogInformation("GetClassifiedContractById method initiated");
            var foundContract = await _mediator.Send(new GetClassifiedContractByIdQuery(id));
            _logger.LogInformation("GetClassifiedContractById method performed");
            return Ok(foundContract);
        }
        [HttpPost]
        public async Task<IActionResult> AddContract(ClassifiedContractDTO cont)
        {
            _logger.LogInformation("AddClassifiedContract method initiated");
            var addedContract = await _mediator.Send(new CreateNewClassifiedContractCommand(cont));
            _logger.LogInformation("AddClassifiedContract method performed");
            if (addedContract != null)
                return Ok(true);
            return Ok(false);
        }
        [Route("{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateClassifiedContract([FromRoute]int id, [FromBody] ClassifiedContractDTO cont)
        {
            _logger.LogInformation("UpdateContract method initiated");
            var editedContract = await _mediator.Send(new EditClassifiedContractCommand(id, cont));
            _logger.LogInformation("UpdateContract method performed");
            return Ok(editedContract); // bool
        }
        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteClassifiedContract([FromRoute] int id)
        {
            _logger.LogInformation("DeleteClassifiedContract method initiated");
            var deletedContract = await _mediator.Send(new RemoveClassifiedContractCommand(id));
            _logger.LogInformation("DeleteClassifiedContract method performed");
            return Ok(deletedContract); // bool
        }
    }
}
