using CMS.Application.Features.ApprovalMatrixMOU.Commands.UpdateApprovalMatrixMOU;
using CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOU;
using CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOUById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalMatrixMOUController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ApprovalMatrixMOUController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Route("{pageNumber}/{pageSize}")]
        [HttpGet]
        public async Task<IActionResult> GetApprovalMatrixMOU([FromRoute] int pageNumber, [FromRoute] int pageSize)
        {
            IEnumerable<GetAllApprovalMatrixMOUDto> approvalMatrixContract = await _mediator.Send(new GetAllApprovalMatrixMOUQuery(pageNumber, pageSize));
            return Ok(approvalMatrixContract);
        }
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetApprovalMatrixContractById([FromRoute] int id)
        {
            GetAllApprovalMatrixMOUByIdDto approvalMatrixContract = await _mediator.Send(new GetAllApprovalMatrixMOUByIdQuery(id));
            return Ok(approvalMatrixContract);
        }

        [HttpPost("UpdateApprovalMatrixMOU")]
        public async Task<IActionResult> UpdateApprovalMatrixMOU(int id, UpdateApprovalMatrixMOUDto mou)
        {
            var updatedApprovalMatrixMOU = await _mediator.Send(new UpdateApprovalMatrixMOUCommand(id, mou));
            return Ok(updatedApprovalMatrixMOU);
        }
    }
}