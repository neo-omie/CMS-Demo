using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.MasterDocuments.Command.UploadDocument;
using CMS.Domain.Entities;

namespace CMS.Application.Contracts.Persistence
{
    public interface IDocumentRepository
    {

        Task<(IEnumerable<MasterDocument> Documents, int TotalCount)> GetAllDocuments(int pageNumber, int pageSize);
        Task<MasterDocument> GetDocumentById(int id);
        Task<int> AddDocument(MasterDocument masterDocument);
        Task<string> UploadDocument(DocumentUploadDto model);

        Task<int> UpdateDocument(int id, MasterDocument masterDocument);
        Task<int> DeleteDocument(int id);
    }
}
