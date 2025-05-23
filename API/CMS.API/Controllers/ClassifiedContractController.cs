using CMS.Application.Features.ClassifiedContracts;
using CMS.Application.Features.ClassifiedContracts.Commands.ApproveRejectContract;
using CMS.Application.Features.ClassifiedContracts.Commands.CreateNewClassifiedContract;

//using CMS.Application.Features.ClassifiedContracts.Commands.CreateNewContract;
using CMS.Application.Features.ClassifiedContracts.Commands.EditClassifiedContract;
using CMS.Application.Features.ClassifiedContracts.Commands.RemoveClassifiedContract;
using CMS.Application.Features.ClassifiedContracts.Queries.GetAllClassifiedContracts;
using CMS.Application.Features.ClassifiedContracts.Queries.GetClassifiedContractById;
using CMS.Application.Features.Contracts;
using CMS.Application.Features.Contracts.Commands.ApproveRejectContract;
using CMS.Application.Features.Contracts.Commands.CreateNewContract;
using CMS.Application.Features.Contracts.Commands.EditContract;
using CMS.Application.Features.Contracts.Queries.GetAllContracts;
using CMS.Application.Features.Contracts.Queries.GetContractByContractName;
using CMS.Application.Features.Contracts.Queries.GetContractById;
using CMS.Application.Features.ContractTypeMaster.Command.DeleteContract;
using CMS.Domain.Constants;
using CMS.Domain.Entities;
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
        public async Task<IActionResult> GetClassifiedContractById([FromRoute] int id)
        {
            _logger.LogInformation("GetClassifiedContractById method initiated");
            var foundContract = await _mediator.Send(new GetClassifiedContractByIdQuery(id));
            _logger.LogInformation("GetClassifiedContractById method performed");
            return Ok(foundContract);
        }
        //[Route("{id}")]
        //[HttpGet]
        //public async Task<IActionResult> GetClassifiedContractById([FromRoute] string id)
        //{
        //    bool isId = int.TryParse(id, out int actualId);
        //    _logger.LogInformation("GetContractById method initiated");
        //    GetClassifiedContractByIdDto foundContract = null;
        //    if (isId)
        //    {
        //        foundContract = await _mediator.Send(new GetClassifiedContractByIdQuery(actualId));
        //    }
        //    else
        //    {
        //        foundContract = await _mediator.Send(new GetContractByContractNameQuery(id));
        //    }
        //    _logger.LogInformation("GetContractById method performed");
        //    return Ok(foundContract);
        //}
        //[HttpPost]
        [HttpPost("${empName}")]
        public async Task<IActionResult> AddContract(ClassifiedContractDTO cont,[FromRoute]string empName)
        {
            _logger.LogInformation("AddClassifiedContract method initiated");
            var addedContract = await _mediator.Send(new CreateNewClassifiedContractCommand(cont, empName));
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

        [Route("{id}/approveRejectContract/{empCode}/{status}")]
        [HttpPost]
        public async Task<IActionResult> ContractApprove([FromRoute] int id, [FromRoute] string empCode, [FromRoute] int status)
        {
            _logger.LogInformation("Classified Contract Approve method initiated");
            ClassifiedContract contract = null;
            if ((ContractStatus)status == ContractStatus.Active)
            {
                contract = await _mediator.Send(new ApproveRejectClassifiedContractCommand(id, empCode, ContractStatus.Active));
            }
            else if ((ContractStatus)status == ContractStatus.Rejected)
            {
                contract = await _mediator.Send(new ApproveRejectClassifiedContractCommand(id, empCode, ContractStatus.Rejected));
            }
            else
            {
                return BadRequest("Wrong contract status number");
            }
            _logger.LogInformation("Classified Contract Approve method performed");
            if (contract != null)
                return Ok(true);
            return Ok(false);
        }
    }
}
