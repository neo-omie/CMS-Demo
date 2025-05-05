using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Exceptions;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace CMS.Persistence.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly CMSDbContext _context;

        public DocumentRepository(CMSDbContext context)
        {
            _context = context;
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
            string sql = "EXEC SP_AddAndUpdateDocument @valueId={0},@documentName={1},@status={2},@documentType={3},@documentData={4},@isDeleted={5}";
           int affectedRows = await _context.Database.ExecuteSqlRawAsync(sql,null,masterDocument.DocumentName, masterDocument.status,masterDocument.DocumentType,masterDocument.DocumentData,masterDocument.IsDeleted);
            if (affectedRows < 0)
            {
            throw new Exception("Something went wrong try again later");
                
            }
            return affectedRows;
        }


        public Task<int> UploadDocument(MasterDocument masterDocument)
        {
            _context.MasterDocuments.AddAsync(masterDocument);
            return _context.SaveChangesAsync();
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
            string sql = "EXEC SP_AddAndUpdateDocument @valueId={0},@documentName={1},@status={2},@documentType={3},@documentData={4},@isDeleted={5}";
            int affectedRows = await  _context.Database.ExecuteSqlRawAsync(sql,id,masterDocument.DocumentName, masterDocument.status, masterDocument.DocumentType, masterDocument.DocumentData, masterDocument.IsDeleted);

            if (affectedRows > 0)
            {
                return affectedRows;
            }
            throw new Exception("Something went wrong try again later");
           
        }

        
    }
}
