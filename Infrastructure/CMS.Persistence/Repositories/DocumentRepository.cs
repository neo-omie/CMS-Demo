using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Exceptions;
using CMS.Application.Features.MasterDocuments.Command.UploadDocument;
using CMS.Domain.Constants;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace CMS.Persistence.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly CMSDbContext _context;

        private readonly IWebHostEnvironment _environment;
        public DocumentRepository(CMSDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<(IEnumerable<MasterDocument> , int )> GetAllDocuments(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("Page size must be greater than 0.");
            }

            
            var totalCount = await _context.MasterDocuments.Where(x => x.IsDeleted == false).CountAsync();
            string sql = "EXEC SP_GetAllDocuments @PageNumber = {0}, @PageSize = {1}";
            var docs = _context.MasterDocuments.FromSqlRaw(sql, pageNumber, pageSize);

            //var docs = _context.MasterDocuments.ToListAsync();
            return (docs, totalCount);
        }

        public async Task<MasterDocument> GetDocumentById(int id)
        { 
            var document =await _context.MasterDocuments.FindAsync(id);
            if (document == null)
            {
                throw new DocumentNotFoundException($"Document with id {id} not found");
            }
            return document;
        }



        public async Task<string> UploadDocument(DocumentUploadDto model)
        {
            if (model.File == null || model.File.Length == 0)
            {
                throw new Exception("No file uploaded.");
            }

            const long maxFileSize = 25 * 1024 * 1024;
            if (model.File.Length > maxFileSize)
            {
                throw new Exception("File size exceeds the 25MB limit.");
            }

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(model.File.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Unsupported file format. Allowed formats: .pdf, .doc, .docx, .jpg and .png.");
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var existingDocument = await _context.MasterDocuments
                .FirstOrDefaultAsync(d => d.DisplayDocumentName == model.File.FileName);

            var originalFileName = Path.GetFileName(model.File.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, originalFileName);

            if (existingDocument != null)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath); 
                }
            }
            else
            {
                //var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                filePath = Path.Combine(uploadsFolder, originalFileName);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            if (existingDocument != null)
            {
                existingDocument.DocumentPath = filePath;
                existingDocument.DisplayDocumentName = originalFileName;
                existingDocument.UniqueDocumentName = uniqueFileName;
                //existingDocument.UniqueDocumentName = Path.GetFileName(filePath);
                _context.MasterDocuments.Update(existingDocument);
            }
            else
            {
                var document = new MasterDocument
                {
                    DocumentPath = filePath,
                    DisplayDocumentName = originalFileName,
                    //UniqueDocumentName = Path.GetFileName(filePath),
                    UniqueDocumentName =uniqueFileName,
                    status = model.Status
                };
                await _context.MasterDocuments.AddAsync(document);
            }

            if (await _context.SaveChangesAsync() > 0)
            {
                return "Document uploaded successfully";
            }

            throw new Exception("For some reasons, document not uploaded");
        }


        public async Task<bool> DeleteDocument(int id)
        {
            var document = await GetDocumentById(id);
            if (document == null)
            {
                throw new Exception("Document not found.");
            }

            if (File.Exists(document.DocumentPath))
            {
                try
                {
                    File.Delete(document.DocumentPath);
                }
                catch (IOException ex)
                {
                    throw new Exception($"Failed to delete file '{document.DocumentPath}': {ex.Message}");
                }
            }
            else
            {
                throw new Exception($"File '{document.DisplayDocumentName}' does not exist.");
            }

            document.IsDeleted = true;
            _context.Remove(document);
            //_context.Update(document);
            if (await _context.SaveChangesAsync() <= 0)
            {
                throw new Exception("Failed to update document status in the database.");
            }

            return true;
        }
        

        
      

        
        public async Task<bool> UpdateDocument(int id, DocumentFormDTO model)
        {
            if (model.File == null || model.File.Length == 0)
            {
                throw new Exception("No file uploaded.");
            }

            const long maxFileSize = 25 * 1024 * 1024;
            if (model.File.Length > maxFileSize)
            {
                throw new Exception("File size exceeds the 25MB limit.");
            }

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(model.File.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Unsupported file format. Allowed formats: .pdf, .doc, .docx, .jpg and .png.");
            }

            // Retrieve the existing document
            var existingDocument = await _context.MasterDocuments.FindAsync(id);
            if (existingDocument == null)
            {
                throw new Exception("Document not found.");
            }

            // Delete the old file if exists
            var oldFilePath = existingDocument.DocumentPath;
            if (File.Exists(oldFilePath))
            {
                try
                {
                    File.Delete(oldFilePath);
                }
                catch (IOException ex)
                {
                    throw new Exception($"Failed to delete old file: {ex.Message}");
                }
            }

            // Prepare uploads folder
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate unique file name and save new file
            //var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var originalFileName = Path.GetFileName(model.File.FileName);

            var newFilePath = Path.Combine(uploadsFolder, originalFileName);

            using (var stream = new FileStream(newFilePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            // Update document properties
            existingDocument.DocumentPath = newFilePath;
            existingDocument.DisplayDocumentName = Path.GetFileName(model.File.FileName);
            //existingDocument.UniqueDocumentName = uniqueFileName;
            existingDocument.status = (Status)model.Status;

            _context.MasterDocuments.Update(existingDocument);
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            throw new Exception("Failed to update document.");

        }
    }
}
