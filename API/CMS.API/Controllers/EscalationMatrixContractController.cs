using CMS.Application.Features.ApprovalMatrixContract.Queries.GetApprovalMatrixContractById;
using CMS.Application.Features.MasterEscalationMatrixContracts;
using CMS.Application.Features.MasterEscalationMatrixContracts.Command;
using CMS.Application.Features.MasterEscalationMatrixContracts.Queries.GetAllEscalationMatrixContracts;
using CMS.Application.Features.MasterEscalationMatrixContracts.Queries.GetEscalationMatrixContractById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin , Super_Admin , Management_User")]
    public class EscalationMatrixContractController : Controller
    {
        private readonly IMediator _mediator;

        public EscalationMatrixContractController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetAllmatrixContracts([FromRoute] int pageNumber, [FromRoute] int pageSize)
        {
            var (contracts, totalcount) = await _mediator.Send(new GetAllEscalationMatrixContractQuery(pageNumber, pageSize));
            var matrixContract = new MatrixContractresponse
            {
                getEscalationMatrixContractDto = contracts,
                TotalCount = totalcount
            };
            return Ok(matrixContract);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatrixContractById(int id)
        {
            var contract = await _mediator.Send(new GetEscalationMatrixContractByIdQuery(id));
            return Ok(contract);
        }


        [HttpPost("{id}/{empCode}")]
        public async Task<IActionResult> UpdateMatrixContract( int id, [FromBody] UpdateEscalationMatrixContractDto updateDto,[FromRoute]string empCode)
        {
            if (updateDto.EscalationId1 == updateDto.EscalationId2 || updateDto.EscalationId1 == updateDto.EscalationId3 || updateDto.EscalationId2 == updateDto.EscalationId3)
            {
                throw new Exception("Escalators cannot be same");
            }
            await _mediator.Send(new UpdateEscalationMatrixContractCommand(id, updateDto,empCode));
            return Ok(new {Message = "SuccessFully updated"});
        }

    }
}
