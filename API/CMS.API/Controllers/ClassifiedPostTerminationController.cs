
using CMS.Application.Features.ClassifiedPostTermination.Command.AddCommand;
using CMS.Application.Features.ClassifiedPostTermination.Command.ApproveTerminationContract;
using CMS.Domain.Constants;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin , Super_Admin , Management_User")]
    public class ClassifiedPostTerminationController : ControllerBase
    {
        readonly IMediator _mediator;
        private readonly CMSDbContext _context;
        readonly ILogger<ClassifiedPostTerminationController> _logger;

        private readonly IWebHostEnvironment _environment;

        public ClassifiedPostTerminationController(IMediator mediator,IWebHostEnvironment environment,CMSDbContext context, ILogger<ClassifiedPostTerminationController> logger)
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

        [Route("approveTerminationContract/{id}/{empCode}/{status}/{subject}/{emailBody}")]
        [HttpPost]
        public async Task<IActionResult> ContractTermination([FromRoute] int id, [FromRoute] string empCode, [FromRoute] int status, [FromRoute] string subject, [FromRoute] string emailBody)
        {
            _logger.LogInformation("Classified ContractTermination method initiated");
            ClassifiedContract contract = null;
            if ((ContractStatus)status == ContractStatus.ApprovedForTermination)
            {
                contract = await _mediator.Send(new ApproveTerminateContractCommand(id, empCode, ContractStatus.ApprovedForTermination,subject,emailBody));
            }
            else if ((ContractStatus)status == ContractStatus.Active)
            {
                contract = await _mediator.Send(new ApproveTerminateContractCommand(id, empCode, ContractStatus.Active, subject, emailBody));
            }
            else
            {
                return BadRequest("Wrong contract status number");
            }
            _logger.LogInformation("Classified ContractTermination method performed");
            if (contract != null)
                return Ok(true);
            return Ok(false);
        }


    }
}
