using System.Reflection.Metadata;
using Azure.Core;
using CMS.Application.Features.Document;
using CMS.Application.Features.MasterDocuments;
using CMS.Application.Features.MasterDocuments.Command.AddDocument;
using CMS.Application.Features.MasterDocuments.Command.DeleteDocument;
using CMS.Application.Features.MasterDocuments.Command.UpdateDocument;
using CMS.Application.Features.MasterDocuments.Command.UploadDocument;
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


        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument([FromForm] DocumentUploadDto model)
        {
            if (model.File == null || model.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var fileName = Path.GetFileName(model.File.FileName);
        
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            
            var document = new MasterDocument
            {
                DocumentName = $"uploads/{filePath}",
                status = model.Status,
               
                 
            };

            
            _context.MasterDocuments.AddAsync(document);
            await _context.SaveChangesAsync();

            return Ok(new { message = "File uploaded successfully."});
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
        public async Task<ActionResult<int>> AddDocument(DocumentFormDTO documentForm)
        {
            if (documentForm.File == null || documentForm.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            using var memoryStream = new MemoryStream();
            await documentForm.File.CopyToAsync(memoryStream);
            var fileData = memoryStream.ToArray();
            var document = new DocumentDTO
            {
                DocumentName = documentForm.File.FileName,
                DocumentType = documentForm.File.ContentType,
                DocumentData = fileData,
                status = (Status)documentForm.Status
            };
            await _mediator.Send(new AddDocumentCommand(document));
            return Ok(new { Message = "Added Document Successfully" });
        }





        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeleteDocument(int id)
        {
            await _mediator.Send(new DeleteDocumentByIdQuery(id));

            return Ok(new { Message = "Deleted Document Successfully" });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> UpdateDocument(int id, DocumentDTO document)
        {
            await _mediator.Send(new UpdateDocumentCommand(id, document));

            return Ok(new { Message = "updated Document Successfully" });
        }


    }
}
