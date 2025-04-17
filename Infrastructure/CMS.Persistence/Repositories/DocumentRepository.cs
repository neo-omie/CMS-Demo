using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;

namespace CMS.Persistence.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly 
        public async Task<IEnumerable<MasterDocument>> GetAllDocuments()
        {
            return await _context.MasterDocument.ToListAsync();
        }

        public Task<MasterDocument> GetDocumentById(int id)
        {
            throw new NotImplementedException();
        }
        public Task<int> AddDocument(MasterDocument masterDocument)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteDocument(int id, MasterDocument masterDocument)
        {
            throw new NotImplementedException();
        }


        public Task<int> UpdateDocument(MasterDocument masterDocument)
        {
            throw new NotImplementedException();
        }
    }
}
