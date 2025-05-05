using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Constants;
using CMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CMS.Application.Features.MasterDocuments.Command.UploadDocument
{
    public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, MasterDocument>
    {
        //private readonly IWebHostEnvironment _environment;
        // private readonly ApplicationDbContext _context; // if using EF

        //public UploadDocumentCommandHandler(IWebHostEnvironment environment/*, ApplicationDbContext context*/)
        //{
        //    _environment = environment;
        //    // _context = context;
        //}

        public async Task<MasterDocument> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {
            if (request.Status != "Active" && request.Status != "Inactive")
            {
                throw new ArgumentException("Invalid status.");
            }

            //var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            //if (!Directory.Exists(uploadsFolder))
            //    Directory.CreateDirectory(uploadsFolder);

            //var uniqueFileName = ToString() + Path.GetExtension(request.File.FileName);
            //var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            //using (var stream = new FileStream(filePath, FileMode.Create))
            //{
            //    await request.File.CopyToAsync(stream, cancellationToken);
            //}

            //var document = new MasterDocument
            //{
            //    DocumentName = request.DocumentName,
            //    status = request.Status,
            //    FilePath = filePath
            //};

            // Save to DB here if needed
            // _context.Documents.Add(document);
            // await _context.SaveChangesAsync(cancellationToken);

            return null;
        }
    }

}