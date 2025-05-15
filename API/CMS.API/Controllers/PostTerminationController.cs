using CMS.Application.Features.Contracts.Commands.ApproveRejectContract;
using CMS.Application.Features.PostTermination.Command.AddCommand;
using CMS.Application.Features.PostTermination.Command.ApproveTerminationContract;
using CMS.Domain.Constants;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostTerminationController : ControllerBase
    {
        readonly IMediator _mediator;
        private readonly CMSDbContext _context;
        readonly ILogger<PostTerminationController> _logger;

        private readonly IWebHostEnvironment _environment;

        public PostTerminationController(IMediator mediator,IWebHostEnvironment environment,CMSDbContext context, ILogger<PostTerminationController> logger)
        {
            _mediator = mediator;
            _environment = environment;
            _context = context;
            _logger = logger;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> uploadDocument([FromForm] int contractId, [FromForm] TerminationDocumentUploadDto _terminationDocumentUploadDto)
        {
            var uploadDoc = await _mediator.Send(new AddPostTerminationCommand(contractId, _terminationDocumentUploadDto));
            return Ok( uploadDoc );
        }

        [Route("{id}/approveRejectContract/{empCode}/{status}/{subject}/{emailBody}")]
        [HttpPost]
        public async Task<IActionResult> ContractTermination([FromRoute] int id, [FromRoute] string empCode, [FromRoute] int status, [FromRoute] string subject, [FromRoute] string emailBody)
        {
            _logger.LogInformation("ContractTermination method initiated");
            Contract contract = null;
            if ((ContractStatus)status == ContractStatus.Terminated)
            {
                contract = await _mediator.Send(new ApproveTerminateContractCommand(id, empCode, ContractStatus.Terminated,subject,emailBody));
            }
            else if ((ContractStatus)status == ContractStatus.Rejected)
            {
                contract = await _mediator.Send(new ApproveTerminateContractCommand(id, empCode, ContractStatus.Rejected, subject, emailBody));
            }
            else
            {
                return BadRequest("Wrong contract status number");
            }
            _logger.LogInformation("ContractTermination method performed");
            if (contract != null)
                return Ok(true);
            return Ok(false);
        }


    }
}
