using CMS.Application.Features.Document;
using CMS.Application.Features.MasterDocuments;
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
        
        [HttpGet("{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetAllDocs([FromRoute]int pageNumber, [FromRoute] int pageSize)
        {

            var(documents, totalCount)  = await _mediator.Send(new GetAllDocumentQuery( pageNumber,  pageSize));

            var getDocs = new DocumentResponse
            {
                Documents = documents,
                TotalCount = totalCount
            };
            return Ok(getDocs);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<MasterDocument>> GetDocumentbyId(int id)
        {

            var getDocs = await _mediator.Send(new GetDocumentByIdQuery(id));
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
