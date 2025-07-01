using CMS.Application.Features.AddendumContracts.Queries.GetAllAddendumContracts;
using CMS.Application.Features.AuditTrails.Queries.GetAllAudits;
using CMS.Application.Features.Contracts.Queries.GetAllContracts;
using CMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin , Super_Admin")]
    
    public class AuditTrailController : ControllerBase
    {
        private readonly IMediator _mediator;
        readonly ILogger<AuditTrailController> _logger;
        public AuditTrailController(IMediator mediator, ILogger<AuditTrailController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        
        [HttpGet("{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetAllAudits([FromRoute]int pageNumber, [FromRoute] int pageSize)
        {
            _logger.LogInformation("GetAllAudits method initiated");
            var allContracts = await _mediator.Send(new GetAllAuditsQuery(pageNumber, pageSize));
          
            
            _logger.LogInformation("GetAllAudits method performed");
            return Ok(allContracts);
        }

    }
}
