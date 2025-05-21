

using CMS.Application.Features.ClassifiedNoticeWithdraw.Command.AddNoticeWithdrawalDetails;
using CMS.Application.Features.ClassifiedNoticeWithdraw.Command.ApproveNoticeWithdrawal;
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
    public class ClassifiedNoticeWithdrawalController : ControllerBase
    {
        readonly IMediator _mediator;
        private readonly CMSDbContext _context;
        readonly ILogger<ClassifiedNoticeWithdrawalController> _logger;
        private readonly IWebHostEnvironment _environment;
        public ClassifiedNoticeWithdrawalController(IMediator mediator, IWebHostEnvironment environment, CMSDbContext context, ILogger<ClassifiedNoticeWithdrawalController> logger)
        {
            _mediator = mediator;
            _environment = environment;
            _context = context;
            _logger = logger;
        }
        [HttpPost("WithdrawalUpload")]
        public async Task<IActionResult> AddWithdrawalNotice([FromForm] int contractId, [FromForm] int postTermId, [FromForm] NoticeWithdrawalDocumentUploadDto dto)
        {
            _logger.LogInformation("Add Withdrawal Notice method initiated");
            var addNotice = await _mediator.Send(new AddNoticeWithdrawalDetailsCommand(contractId, postTermId, dto));
            _logger.LogInformation("Add Withdrawal Notice method performed");
            return Ok(addNotice);
        }
        [Route("approveWithdrawalNotice/{id}/{empCode}/{status}/{subject}/{emailBody}")]
        [HttpPost]
        public async Task<IActionResult> ApproveNoticeWithdrawal([FromRoute] int id, [FromRoute] string empCode, [FromRoute] ContractStatus status, [FromRoute] string subject, [FromRoute] string emailBody)
        {
            _logger.LogInformation("Withdrawal Approve method initiated");
            ClassifiedContract approveWithdrawal = null;
            if ((ContractStatus)status == ContractStatus.Active) // Approved Withdrawal
            {
                approveWithdrawal = await _mediator.Send(new ApproveNoticeWithdrawalCommand(id, empCode, ContractStatus.Active, subject, emailBody));
            }
            else if ((ContractStatus)status == ContractStatus.ApprovedForTermination) // Rejected Withdrawal
            {
                approveWithdrawal = await _mediator.Send(new ApproveNoticeWithdrawalCommand(id, empCode, ContractStatus.ApprovedForTermination, subject, emailBody));
            }
            else
            {
                return BadRequest("Wrong contract status number");
            }
            _logger.LogInformation("Withdrawal Approve method performed");
            if (approveWithdrawal != null)
                return Ok(true);
            return Ok(false);
        }
    }
}
