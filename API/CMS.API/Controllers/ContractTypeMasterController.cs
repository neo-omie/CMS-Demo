﻿using AutoMapper;
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
using CMS.Application.Features.MasterCompanies.Query.GetCompanyById;
using CMS.Application.Features.ContractTypeMaster.Query.GetContractById;
using CMS.Application.Features.ContractTypeMaster.Query;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractTypeMasterController : ControllerBase
    {
        readonly IMediator _mediator;
        readonly IMapper _mapper;
        readonly ILogger<ContractTypeMasterController> _logger;

        public ContractTypeMasterController(IMediator mediator, IMapper mapper, ILogger<ContractTypeMasterController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        //Get all Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllContractTypesDTO>>> GetAllContracts(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation("GetAllContracts method initiated");
            var query = new GetAllContractQuery
            (
                pageNumber, pageSize
            );
            var runQuery = await _mediator.Send(query);
            _logger.LogInformation("GetAllContracts method Performed");
            return Ok(runQuery);
        }

        //Get company by ID
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetContractById([FromRoute] int id)
        {
            _logger.LogInformation("GetContractById method initiated");
            var contract = await _mediator.Send(new GetContractByIdQuery(id));
            _logger.LogInformation("GetContractById method Performed");
            return Ok(contract);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<ContractTypeMasters>>> AddContract([FromBody] AddContractDTO cont)
        {
            _logger.LogInformation("AddContracts method initiated");
            var command = new AddContractCommand(cont);
            _logger.LogInformation("AddContracts method Performed");
            return Ok(await _mediator.Send(command));

        }

         [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<ContractTypeMasters>>> UpdateContract(int id, [FromBody] UpdateContractDTO conty)
        {
            _logger.LogInformation("UpdateContracts method initiated");
            var command = new UpdateContractCommand(id, conty);
            _logger.LogInformation("UpdateContracts method Performed");
            return Ok(await _mediator.Send(command));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContract(int id)
        {
            _logger.LogInformation("DeleteContracts method initiated");
            var command = new DeleteContractCommand(id);
            var checkifDel = await _mediator.Send(command);
            if (checkifDel)
                return Ok(checkifDel);
            _logger.LogInformation("DeleteContracts method Performed");
            return BadRequest();
        }
    }
}
