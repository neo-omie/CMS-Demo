using AutoMapper;
using CMS.Application.Features.MasterCompanies;
using CMS.Application.Features.MasterCompanies.Command.AddCompany;
using CMS.Application.Features.MasterCompanies.Command.DeleteCompany;
using CMS.Application.Features.MasterCompanies.Command.UpdateCompany;
using CMS.Application.Features.MasterCompanies.Query.GetAllCompanies;
using CMS.Application.Features.MasterEmployees.Queries.GetAllEmployees;
using CMS.Domain.Entities;
using CMS.Domain.Entities.CompanyMaster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterCompanyController : ControllerBase
    {
        readonly IMediator _mediator;
        readonly IMapper _mapper;
       

        public MasterCompanyController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }


        //Get all Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MasterCompany>>> GetAllCompanies(
        [FromQuery] string searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
        {
            var query = new GetAllCompaniesQuery
            (
               searchTerm, pageNumber, pageSize
            );

            var compDto = _mapper.Map <GetMastersDTO> (query);

            return Ok(await _mediator.Send(compDto));
        }


        //Add
        [HttpPost]
        public async Task<ActionResult<MasterCompany>> AddCompany([FromBody] MasterCompany company)
        {
            var command = new AddCompanyCommand(company);
            return Ok(await _mediator.Send(command));
        }

        //update
        [HttpPut("{id}")]
        public async Task<ActionResult<MasterCompany>>UpdateCompany(int id, [FromBody] MasterCompany company)
        {
            var command = new UpdateCompanyCommand(id, company);
            return Ok(await _mediator.Send(command));
        }

        //Delete 

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var command = new DeleteCompanyCommand(id);
            var checkifDel = await _mediator.Send(command);
            if (checkifDel)
                return Ok("successfully deleted");
            return BadRequest();
        }


    }
}
