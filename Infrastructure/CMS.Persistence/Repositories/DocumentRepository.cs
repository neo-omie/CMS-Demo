using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Exceptions;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
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

        //public async Task<IEnumerable<MasterDocument>> GetAllDocuments(int pageNumber, int pageSize)
        //{
        //    return await _context.MasterDocuments.Skip((pageNumber - 1) * pageSize).Take(pageSize)
        //        .ToListAsync();
        //}

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

            
            var totalCount = await _context.MasterDocuments.CountAsync();


            var documents = await _context.MasterDocuments
                .Where(x => x.IsDeleted == false)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
                

            return (documents, totalCount);
        }

        public async Task<MasterDocument> GetDocumentById(int id)
        {
            var document = await _context.MasterDocuments.FirstOrDefaultAsync(x => x.ValueId == id);
            if (document == null)
            {
                throw new DocumentNotFoundException("Document not found");
            }
            return document;
        }
        public async Task<int> AddDocument(MasterDocument masterDocument)
        {
            await _context.MasterDocuments.AddAsync(masterDocument);
            return _context.SaveChanges();
        }

        public async Task<int> DeleteDocument(int id)
        {
            var document = await GetDocumentById(id);
            document.IsDeleted = true;
            _context.MasterDocuments.Update(document);
           return await _context.SaveChangesAsync();
            //return _context.SaveChanges();
        }


        public async Task<int> UpdateDocument(int id, MasterDocument masterDocument)
        {
            var foundDoc = await _context.MasterDocuments.FirstOrDefaultAsync(md => md.ValueId == id);
            if(foundDoc == null)
            {
                throw new NotFoundException($"Document with ID {id} not found. Please enter correct ID.");
            }
            foundDoc.DocumentName = masterDocument.DocumentName;
            foundDoc.status = masterDocument.status;
             _context.MasterDocuments.Update(foundDoc);
            return await _context.SaveChangesAsync();

        }
    }
}
