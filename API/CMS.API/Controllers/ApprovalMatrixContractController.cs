using CMS.Application.Features.ApprovalMatrixContract.Commands;
using CMS.Application.Features.ApprovalMatrixContract.Queries.GetAllApprovalMatrixContract;
using CMS.Application.Features.ApprovalMatrixContract.Queries.GetApprovalMatrixContractById;
using CMS.Application.Features.ApprovalMatrixMOU.Commands.UpdateApprovalMatrixMOU;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalMatrixContractController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ApprovalMatrixContractController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Route("{pageNumber}/{pageSize}")]
        [HttpGet]
        public async Task<IActionResult> GetApprovalMatrixContract([FromRoute]int pageNumber, [FromRoute]int pageSize)
        {
            IEnumerable<GetAllApprovalMatrixContractDTO> approvalMatrixContract = await _mediator.Send(new GetAllApprovalMatrixContractQuery(pageNumber, pageSize));
            return Ok(approvalMatrixContract);
        }
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetApprovalMatrixContractById([FromRoute] int id)
        {
            GetApprovalMatrixContractByIdDto approvalMatrixContract = await _mediator.Send(new GetApprovalMatrixContractByIdQuery(id));
            return Ok(approvalMatrixContract);
        }
        [Route("{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateApprovalMatrixContract([FromRoute]int id,[FromBody]UpdateApprovalMatrixContractDto contract)
        {
            var updatedApprovalMatrixContract = await _mediator.Send(new UpdateApprovalMatrixContractCommand(id, contract));
            return Ok(updatedApprovalMatrixContract);
        }
    }
}
