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
        readonly ILogger<MasterCompanyController> _logger;
       

        public MasterCompanyController(IMediator mediator, IMapper mapper, ILogger<MasterCompanyController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }


        //Get all Companies
        [HttpGet("{pageNumber}/{pageSize}")]
        public async Task<ActionResult<IEnumerable<GetMastersDTO>>> GetAllCompanies(
        [FromQuery] string searchTerm, int pageNumber = 1, int pageSize = 10)
        {
            _logger.LogInformation("GetAllCompany method initiated");
            var query = new GetAllCompaniesQuery
            (
               searchTerm, pageNumber, pageSize
            );
            var runQuery = await _mediator.Send(query);
            var compDto = _mapper.Map<IEnumerable<GetMastersDTO>>(runQuery);
            _logger.LogInformation("GetAllCompany method Performed");
            return Ok(compDto);
        }


        //Add
        [HttpPost]
        public async Task<ActionResult<MasterCompany>> AddCompany([FromBody] MasterCompany company)
        {
            _logger.LogInformation("AddCompany method initiated");
            var command = new AddCompanyCommand(company);
            _logger.LogInformation("AddCompany method Performed");
            return Ok(await _mediator.Send(command));

        }

        //update
        [HttpPut("{id}")]
        public async Task<ActionResult<MasterCompany>>UpdateCompany(int id, [FromBody] MasterCompany company)
        {
            _logger.LogInformation("UpdateCompany method initiated");
            var command = new UpdateCompanyCommand(id, company);
            _logger.LogInformation("UpdateCompany method Performed");
            return Ok(await _mediator.Send(command));
        }

        //Delete 

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            _logger.LogInformation("DeleteCompany method initiated");
            var command = new DeleteCompanyCommand(id);
            var checkifDel = await _mediator.Send(command);
            if (checkifDel)
                return Ok("successfully deleted");
            _logger.LogInformation("DeleteCompany method Performed");
            return BadRequest();
        }


    }
}
