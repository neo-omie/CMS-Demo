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

        public async Task<(IEnumerable<MasterDocument> docs, int totalCount)> GetAllDocuments(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("Page size must be greater than 0.");
            }

            
            var totalCount = await _context.MasterDocuments.CountAsync();
            string sql = "EXEC SP_GetAllDocuments @PageNumber = {0}, @PageSize = {1}";
            var docs =await  _context.MasterDocuments.FromSqlRaw(sql, pageNumber, pageSize).ToListAsync();

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



        public async Task<string> UploadDocument(DocumentUploadDto model,string empCode)
        {

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(model.File.FileName).ToLowerInvariant();
            int id,res =-1;
            MasterDocument gotDocument  =null;
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Unsupported file format. Allowed formats: .pdf, .doc, .docx, .jpg and .png."); 
            }
            if (model.File == null || model.File.Length == 0)
            {
                throw new Exception("No file uploaded.");
            }

            const long maxFileSize = 25 * 1024 * 1024;
            if (model.File.Length > maxFileSize)
            {
                throw new Exception("File size exceeds the 25MB limit.");
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

                id = existingDocument.ValueId;
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
                 gotDocument =await _context.MasterDocuments.OrderBy(x => x.ValueId).LastAsync();
                id = gotDocument.ValueId;
            }
                res=  await _context.SaveChangesAsync();

            if (res > 0)
            {
                string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
                await _context.Database.ExecuteSqlRawAsync(query, id , TableList.MasterDocument, $"New Document '{originalFileName}' has been Added by '{ empCode}'", empCode, LogStatus.Created);

                return "Document uploaded successfully";
            }

            throw new Exception("For some reasons, document not uploaded");
        }


        public async Task<bool> DeleteDocument(int id,string empCode)
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

            //document.IsDeleted = true;
            _context.Remove(document);
            //_context.Update(document);
            if (await _context.SaveChangesAsync() <= 0)
            {
                throw new Exception("Failed to update document status in the database.");
            }

            string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
            await _context.Database.ExecuteSqlRawAsync(query, id, TableList.MasterDocument, $"Document '{document.DisplayDocumentName}' has been Deleted by '{ empCode}'", empCode, LogStatus.Deleted);


            return true;
        }
        
        
        public async Task<object> UpdateDocument(int id, DocumentFormDTO model,string empCode)
        {
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(model.File.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Unsupported file format. Allowed formats: .pdf, .doc, .docx, .jpg and .png.");
            }
            if (model.File == null || model.File.Length == 0)
            {
                throw new Exception("No file uploaded.");
            }

            const long maxFileSize = 25 * 1024 * 1024;
            if (model.File.Length > maxFileSize)
            {
                throw new Exception("File size exceeds the 25MB limit.");
            }

            // Prepare uploads folder
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate unique file name and save new file
            var originalFileName = Path.GetFileName(model.File.FileName);

            var newFilePath = Path.Combine(uploadsFolder, originalFileName);

            //var existingDocument = await _context.MasterDocuments.FirstOrDefaultAsync(d => d.DisplayDocumentName == model.File.FileName);
            var existingDocument = _context.MasterDocuments.Where(d => d.DisplayDocumentName == model.File.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var currDocument = await GetDocumentById(id);
            _context.MasterDocuments.Remove(currDocument);

            if (existingDocument != null)
            {
                await existingDocument.ForEachAsync(d => {
                    _context.MasterDocuments.Remove(d);
                   
                if (File.Exists(d.DocumentPath))
                {
                    File.Delete(d.DocumentPath);

                } });

                
                //_context.Master();
                //existingDocument.status = (Status)model.Status;
                //existingDocument.DocumentPath = newFilePath;
                //existingDocument.DisplayDocumentName = originalFileName;
                //existingDocument.IsDeleted = false;

                var document = new MasterDocument
                {
                    DocumentPath = newFilePath,
                    DisplayDocumentName = originalFileName,
                    status = (Status)model.Status,
                    UniqueDocumentName = uniqueFileName
                   
                };
                using (var stream = new FileStream(originalFileName, FileMode.Create))
                {
                   await model.File.CopyToAsync(stream);
                }

                await _context.MasterDocuments.AddAsync(document);
            }
            else
            {
                var document = new MasterDocument
                {
                    DocumentPath = newFilePath,
                    DisplayDocumentName = originalFileName,
                    status = (Status)model.Status,
                    UniqueDocumentName = uniqueFileName,
                    
                };
                using (var stream = new FileStream(originalFileName, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }
                await _context.MasterDocuments.AddAsync(document);
            }

            if (await _context.SaveChangesAsync() > 0)
            {

                string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
                await _context.Database.ExecuteSqlRawAsync(query, id, TableList.MasterDocument, $"Document '{originalFileName}' has been Updated by '{ empCode}'", empCode, LogStatus.Updated);


                return new { Message = "Document updated" };
            }

            throw new Exception("Failed to update document.");

        }
        

        public async Task<bool> CheckFileExists( DocumentFormDTO model)
        {
            var existingDocumentName = await _context.MasterDocuments
                            .FirstOrDefaultAsync(d => d.DisplayDocumentName == model.File.FileName);

            if (existingDocumentName != null)
            {
                return true;
            }
            return false;
        }
    }
}
