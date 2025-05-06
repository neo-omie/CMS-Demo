using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Exceptions;
using CMS.Application.Features.MasterDocuments.Command.UploadDocument;
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

            return (docs, totalCount);
        }

        public async Task<MasterDocument> GetDocumentById(int id)
        {
            string sql = "EXEC SP_GetDocumentByID @id = {0}";
            var findingDocument = await _context.MasterDocuments.FromSqlRaw(sql, id).AsNoTracking().ToListAsync();
            var document = findingDocument.FirstOrDefault();
            if (document == null)
            {
                throw new DocumentNotFoundException($"Document with id {id} not found");
            }
            return document;
        }
        public async Task<int> AddDocument(MasterDocument masterDocument)
        {
            // string sql = "EXEC SP_AddAndUpdateDocument @valueId={0},@documentName={1},@status={2},@documentType={3},@documentData={4},@isDeleted={5}";
            //int affectedRows = await _context.Database.ExecuteSqlRawAsync(sql,null,masterDocument.DocumentName, masterDocument.status,masterDocument.DocumentType,masterDocument.DocumentData,masterDocument.IsDeleted);
            // if (affectedRows < 0)
            // {
            // throw new Exception("Something went wrong try again later");

            // }
            // return affectedRows;
            return 1;
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
                throw new Exception("Unsupported file format. Allowed formats: .pdf, .doc, .docx, .jpg and .png).");
            }

            using var memoryStream = new MemoryStream();
            await model.File.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();
            var fileHash = Convert.ToBase64String(SHA256.Create().ComputeHash(fileBytes));

            var existingDocument = await _context.MasterDocuments
                .FirstOrDefaultAsync(d => d.UniqueDocumentName == fileHash);

            if (existingDocument != null)
            {
                throw new Exception("A document with the same content has already been uploaded.");
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var originalFileName = Path.GetFileName(model.File.FileName);

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            var document = new MasterDocument
            {
                DocumentPath = $"uploads/{filePath}",
                DisplayDocumentName = originalFileName,
                UniqueDocumentName = fileHash,
                status = model.Status
            };

            await _context.MasterDocuments.AddAsync(document);
            if (await _context.SaveChangesAsync() > 0)
                return "Document uploaded successfully";
            throw new Exception("For some reasons, document not uploaded");
        }

        public async Task<int> DeleteDocument(int id)
        {
            string sql = "EXEC SP_DeleteDocumentById @id";

            
            var affectedRows = await _context.Database.ExecuteSqlRawAsync(sql, new SqlParameter("@id", id));

           
            //await _context.SaveChangesAsync();

            return affectedRows; 
        }


        public async Task<int> UpdateDocument(int id, MasterDocument masterDocument)
        {
            //string sql = "EXEC SP_AddAndUpdateDocument @valueId={0},@documentName={1},@status={2},@documentType={3},@documentData={4},@isDeleted={5}";
            //int affectedRows = await  _context.Database.ExecuteSqlRawAsync(sql,id,masterDocument.DocumentName, masterDocument.status, masterDocument.DocumentType, masterDocument.DocumentData, masterDocument.IsDeleted);

            //if (affectedRows > 0)
            //{
            //    return affectedRows;
            //}
            //throw new Exception("Something went wrong try again later");
            return 1;
        }

        
    }
}
