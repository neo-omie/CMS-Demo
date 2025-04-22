using CMS.Application.Features.Document;
using CMS.Application.Features.MasterDocuments.Command.AddDocument;
using CMS.Application.Features.MasterDocuments.Command.DeleteDocument;
using CMS.Application.Features.MasterDocuments.Command.UpdateDocument;
using CMS.Application.Features.MasterDocuments.Queries.GetAllDocument;
using CMS.Application.Features.MasterDocuments.Queries.GetDocumentById;
using CMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : Controller
    {
        readonly IMediator _mediator;

        public DocumentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MasterDocument>>> GetAllDocs()
        {

            var getDocs = _mediator.Send(new GetAllDocumentQuery());
            return Ok(getDocs);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<MasterDocument>> GetDocumentbyId(int id)
        {

            var getDocs = _mediator.Send(new GetDocumentByIdQuery(id));
            return Ok(getDocs);
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddDocument(DocumentDTO document)
        {
            await _mediator.Send(new AddDocumentCommand(document));

            return Ok(new { Message = "Added Document Successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeleteDocument(int id)
        {
            await _mediator.Send(new DeleteDocumentByIdQuery(id));

            return Ok(new { Message = "Deleted Document Successfully" });
        }

        [HttpPut]
        public async Task<ActionResult<int>> UpdateDocument(int id, DocumentDTO document)
        {
            await _mediator.Send(new UpdateDocumentCommand(id, document));

            return Ok(new { Message = "updated Document Successfully" });
        }


    }
}
