using CMS.Application.Features.Contracts;
using CMS.Application.Features.Contracts.Commands.ApproveRejectContract;
using CMS.Application.Features.Contracts.Commands.CreateNewContract;
using CMS.Application.Features.Contracts.Commands.EditContract;
using CMS.Application.Features.Contracts.Commands.RemoveContract;
using CMS.Application.Features.Contracts.Commands.RenewalRequestContract;
using CMS.Application.Features.Contracts.Queries.GetActiveContracts;
using CMS.Application.Features.Contracts.Queries.GetAllContracts;
using CMS.Application.Features.Contracts.Queries.GetContractByContractName;
using CMS.Application.Features.Contracts.Queries.GetContractById;
using CMS.Application.Features.Contracts.Queries.GetContractsCount;
using CMS.Application.Features.Contracts.Queries.GetExpiredContracts;
using CMS.Application.Features.Contracts.Queries.GetPendingApprovalContracts;
using CMS.Application.Features.Contracts.Queries.GetTerminatedContracts;
using CMS.Application.Features.ContractTypeMaster.Command.DeleteContract;
using CMS.Domain.Constants;
using CMS.Domain.Entities;
using CMS.Persistence.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Authorize]
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
        [HttpPost("GetAllContracts/{eCode}")]
        public async Task<IActionResult> GetAllContracts([FromBody]FiltersContractDto filters, [FromRoute]string eCode)
        {
           _logger.LogInformation("GetAllContracts method initiated");
            var allContracts = await _mediator.Send(new GetAllContractsQuery(filters,eCode));
            _logger.LogInformation("GetAllContracts method performed");
            return Ok(allContracts);
        }
        [HttpGet("GetActiveContracts")]
        public async Task<IActionResult> GetActiveContracts(int pageNumber, int pageSize)
        {
            _logger.LogInformation("GetActiveContracts method initiated");
            var activeContracts = await _mediator.Send(new GetActiveContractsQuery(pageNumber, pageSize));
            _logger.LogInformation("GetActiveContracts method performed");
            return Ok(activeContracts);
        }
        [HttpGet("GetTerminatedContracts")]
        public async Task<IActionResult> GetTerminatedContracts(int pageNumber, int pageSize)
        {
            _logger.LogInformation("GetTerminatedContracts method initiated");
            var terminatedContracts = await _mediator.Send(new GetTerminatedContractsQuery(pageNumber, pageSize));
            _logger.LogInformation("GetTerminatedContracts method performed");
            return Ok(terminatedContracts);
        }
        [HttpGet("GetPendingApprovalContracts")]
        public async Task<IActionResult> GetPendingApprovalContracts(int pageNumber, int pageSize)
        {
            _logger.LogInformation("GetPendingApprovalContracts method initiated");
            var pendingApprovalContracts = await _mediator.Send(new GetPendingApprovalContractsQuery(pageNumber, pageSize));
            _logger.LogInformation("GetPendingApprovalContracts method performed");
            return Ok(pendingApprovalContracts);
        }
        [HttpGet("GetExpiredContracts")]
        public async Task<IActionResult> GetExpiredContracts(int pageNumber, int pageSize)
        {
            _logger.LogInformation("GetExpiredContracts method initiated");
            var expiredContracts = await _mediator.Send(new GetExpiredContractsQuery(pageNumber, pageSize));
            _logger.LogInformation("GetExpiredContracts method performed");
            return Ok(expiredContracts);
        }

        [HttpGet("GetContractsCount")]
        public async Task<IActionResult> GetContractsCount()
        {
            _logger.LogInformation("GetContractsCount method initiated");
            var contractCounts = await _mediator.Send(new GetContractsCountQuery());
            _logger.LogInformation("GetContractsCount method performed");
            return Ok(contractCounts);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetContractById([FromRoute]string id)
        {
            bool isId = int.TryParse(id,out int actualId);
            _logger.LogInformation("GetContractById method initiated");
            GetContractByIdDto foundContract = null;
            if (isId)
            {
                foundContract = await _mediator.Send(new GetContractByIdQuery(actualId));
            }
            else
            {
                foundContract = await _mediator.Send(new GetContractByContractNameQuery(id));
            }
                _logger.LogInformation("GetContractById method performed");
            return Ok(foundContract);
        }
        [HttpPost("{empCode}")]
        public async Task<IActionResult> AddContract(ContractDTO cont, [FromRoute]string empCode)
        {
            _logger.LogInformation("AddContract method initiated");
            var addedContract = await _mediator.Send(new CreateNewContractCommand(cont,empCode));
            _logger.LogInformation("AddContract method performed");
            if(addedContract != null)
                return Ok(true);
            return Ok(false);
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
        [Route("{id}/{empCode}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteContract([FromRoute] int id, [FromRoute]string empCode)
        {
            _logger.LogInformation("DeleteContract method initiated");
            var deletedContract = await _mediator.Send(new RemoveContractCommand(id,empCode));
            _logger.LogInformation("DeleteContract method performed");
            return Ok(deletedContract); // bool
        }
        [Route("{id}/approveRejectContract/{empCode}/{status}")]
        [HttpPost]
        public async Task<IActionResult> ContractApprove([FromRoute] int id, [FromRoute] string empCode, [FromRoute] int status)
        {
            _logger.LogInformation("ContractApprove method initiated");
            Contract contract = null;
            if((ContractStatus)status == ContractStatus.Active)
            {
                 contract = await _mediator.Send(new ApproveRejectContractCommand(id, empCode, ContractStatus.Active));
            }
            else if((ContractStatus)status == ContractStatus.Rejected)
            {
                contract = await _mediator.Send(new ApproveRejectContractCommand(id, empCode, ContractStatus.Rejected));
            }
            else
            {
                return BadRequest("Wrong contract status number");
            }
             _logger.LogInformation("ContractApprove method performed");
            if (contract != null)
                return Ok(true);
            return Ok(false);
        }

        [Route("{id}/renewalRequest/{empCode}")]
        [HttpPost]
        public async Task<IActionResult> ContractRenewalRequest([FromRoute] int id, [FromRoute] string empCode)
        {
            Contract contract = await _mediator.Send(new RenewalRequestContractCommand(id, empCode));
            if (contract != null)
                return Ok(true);
            return Ok(false);
        } 
    }
}
