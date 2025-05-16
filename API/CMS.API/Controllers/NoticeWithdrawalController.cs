using CMS.Application.Features.NoticeWithdraw.Command.AddNoticeWithdrawalDetails;
using CMS.Application.Features.NoticeWithdraw.Command.ApproveNoticeWithdrawal;
using CMS.Domain.Constants;
using CMS.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticeWithdrawalController : ControllerBase
    {
        readonly IMediator _mediator;
        private readonly CMSDbContext _context;
        readonly ILogger<NoticeWithdrawalController> _logger;
        private readonly IWebHostEnvironment _environment;
        public NoticeWithdrawalController(IMediator mediator, IWebHostEnvironment environment, CMSDbContext context, ILogger<NoticeWithdrawalController> logger)
        {
            _mediator = mediator;
            _environment = environment;
            _context = context;
            _logger = logger;
        }
        [HttpPost("WithdrawalUpload")]
        public async Task<IActionResult> AddWithdrawalNotice([FromForm] int contractId, [FromForm] int postTermId, [FromForm] NoticeWithdrawalDocumentUploadDto dto)
        {
            var addNotice = await _mediator.Send(new AddNoticeWithdrawalDetailsCommand(contractId, postTermId, dto));
            return Ok(addNotice);
        }
        [Route("approveWithdrawalNotice/{id}/{empCode}/{status}/{subject}/{emailBody}")]
        [HttpPost]
        public async Task<IActionResult> ApproveNoticeWithdrawal([FromRoute] int id, [FromRoute] string empCode, [FromRoute] ContractStatus status, [FromRoute] string subject, [FromRoute] string emailBody)
        {
            var approveWithdrawal = await _mediator.Send(new ApproveNoticeWithdrawalCommand(id, empCode, status, subject, emailBody));
            return Ok(approveWithdrawal);
        }
    }
}
