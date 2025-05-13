using CMS.Application.Features.PostTermination.Command.AddCommand;
using CMS.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostTerminationController : ControllerBase
    {
        readonly IMediator _mediator;
        private readonly CMSDbContext _context;

        private readonly IWebHostEnvironment _environment;

        public PostTerminationController(IMediator mediator,IWebHostEnvironment environment,CMSDbContext context)
        {
            _mediator = mediator;
            _environment = environment;
            _context = context;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> uploadDocument([FromForm] int contractId, [FromForm] TerminationDocumentUploadDto _terminationDocumentUploadDto)
        {
            var uploadDoc = await _mediator.Send(new AddPostTerminationCommand(contractId, _terminationDocumentUploadDto));
            return Ok( uploadDoc );
        }
    }
}
