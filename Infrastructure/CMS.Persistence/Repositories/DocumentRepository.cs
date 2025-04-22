using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
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

        public async Task<IEnumerable<MasterDocument>> GetAllDocuments()
        {
            return await _context.MasterDocuments.ToListAsync();
        }

        public async Task<MasterDocument> GetDocumentById(int id)
        {
            var document = await _context.MasterDocuments.FirstOrDefaultAsync(x => x.ValueId == id);
            if (document == null)
            {
                //throw new DocumentNotFound($"Document not found");                
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


        public async Task<int> UpdateDocument(MasterDocument masterDocument)
        {
           
            _context.MasterDocuments.Update(masterDocument);
            return await _context.SaveChangesAsync();

        }
    }
}
