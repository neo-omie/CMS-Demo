using CMS.Application.Features.ApprovalMatrixContract.Queries.GetAllApprovalMatrixContract;
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
        public async Task<IActionResult> GetApprovalMatrixContractController([FromRoute]int pageNumber, [FromRoute]int pageSize)
        {
            IEnumerable<GetAllApprovalMatrixContractDTO> approvalMatrixContract = await _mediator.Send(new GetAllApprovalMatrixContractQuery(pageNumber, pageSize));
            return Ok(approvalMatrixContract);
        }
    }
}
