using System.Collections;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using Azure.Core;
//using CMS.Application.Features.Document;
using CMS.Application.Features.MasterDocuments;
using CMS.Application.Features.MasterDocuments.Command.DeleteDocument;
using CMS.Application.Features.MasterDocuments.Command.UpdateDocument;
using CMS.Application.Features.MasterDocuments.Command.UploadDocument;
using CMS.Application.Features.MasterDocuments.Queries.CheckFileExists;
using CMS.Application.Features.MasterDocuments.Queries.GetAllDocument;
using CMS.Application.Features.MasterDocuments.Queries.GetDocumentById;
using CMS.Domain.Constants;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : Controller
    {
        readonly IMediator _mediator;
        private readonly CMSDbContext _context;

        private readonly IWebHostEnvironment _environment;
        public DocumentController(IMediator mediator, IWebHostEnvironment environment, CMSDbContext context)
        {
            _mediator = mediator;
            _environment = environment;
            _context = context;
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


        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument([FromForm] DocumentUploadDto model)
        {
            var uploadDoc = await _mediator.Send(new UploadDocumentCommand(model));
            return Ok( new {  Message = uploadDoc});
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeleteDocument(int id)
        {
            await _mediator.Send(new DeleteDocumentByIdQuery(id));

            return Ok(new { Message = "Deleted Document Successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(int id, DocumentFormDTO documentForm)
        {
           
            var result = await _mediator.Send(new UpdateDocumentCommand(id, documentForm));

            return Ok(result);
        }

        [HttpPut("checkFileExists")]
        public async Task<IActionResult> CheckFileExists( DocumentFormDTO documentForm)
        {
            var query = new CheckFileNameExistsQuery(documentForm);
            var exists = await _mediator.Send(query);
            return Ok(exists);
        }


        [Route("{id}/updateWithoutFile")]
        [HttpPut]
        public Task<IActionResult> UpdateDocumentWithoutFile(int id, DocumentUploadWithoutFileDto model)
        {
            var doc = _context.MasterDocuments.Where(d => d.ValueId == id).FirstOrDefault();
            if(doc == null)
            {
                throw new Exception("Document not found");
            }
            doc.status = (Status)model.Status;
            _context.MasterDocuments.Update(doc);
            _context.SaveChanges();
            return Task.FromResult<IActionResult>(Ok(new { Message = "Updated successfully" }));
        }

    }
}
