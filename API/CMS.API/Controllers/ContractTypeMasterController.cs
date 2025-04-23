using AutoMapper;
using CMS.Application.Features.MasterCompanies.Query.GetAllCompanies;
using CMS.Application.Features.MasterCompanies;
using CMS.Domain.Entities.CompanyMaster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CMS.Domain.Entities;
using CMS.Application.Features.ContractTypeMaster;
using CMS.Application.Features.ContractTypeMaster.Query.GetAllContract;
using CMS.Application.Features.MasterCompanies.Command.AddCompany;
using CMS.Application.Features.MasterCompanies.Command.UpdateCompany;
using CMS.Application.Features.MasterCompanies.Command.DeleteCompany;
using CMS.Application.Features.ContractTypeMaster.Command.DeleteContract;
using CMS.Application.Features.ContractTypeMaster.Command.AddContract;
using CMS.Application.Features.ContractTypeMaster.Command.UpdateContract;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractTypeMasterController : ControllerBase
    {
        readonly IMediator _mediator;
        readonly IMapper _mapper;

        public ContractTypeMasterController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        //Get all Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContractTypeMasters>>> GetAllContracts(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
        {
            var query = new GetAllContractQuery
            (
                pageNumber, pageSize
            );

            var contDto = _mapper.Map<GetContractDTO>(query);

            return Ok(await _mediator.Send(contDto));
        }

        [HttpPost]
        public async Task<ActionResult<ContractTypeMasters>> AddContract([FromBody] ContractTypeMasters cont)
        {
            var command = new AddContractCommand(cont);
            return Ok(await _mediator.Send(command));
        }

         [HttpPut("{id}")]
        public async Task<ActionResult<ContractTypeMasters>>UpdateContract(int id, [FromBody] ContractTypeMasters conty)
        {
            var command = new UpdateContractCommand(id, conty);
            return Ok(await _mediator.Send(command));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContract(int id)
        {
            var command = new DeleteContractCommand(id);
            var checkifDel = await _mediator.Send(command);
            if (checkifDel)
                return Ok("successfully deleted");
            return BadRequest();
        }
    }
}
