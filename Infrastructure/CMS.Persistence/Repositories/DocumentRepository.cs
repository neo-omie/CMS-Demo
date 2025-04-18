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
            return await _context.MasterDocuments.FirstOrDefaultAsync(x => x.ValueId == id);
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
            return _context.SaveChanges();
        }


        public async Task<int> UpdateDocument(int id, MasterDocument masterDocument)
        {
            var document = await GetDocumentById(id);
            document.DocumentName = masterDocument.DocumentName;
            document.status = masterDocument.status;

            return _context.SaveChanges();

        }
    }
}
