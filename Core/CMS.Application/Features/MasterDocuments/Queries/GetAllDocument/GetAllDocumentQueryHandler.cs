using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterDocuments.Queries.GetAllDocument
{
    public class GetAllDocumentQueryHandler : IRequestHandler<GetAllDocumentQuery, (IEnumerable<MasterDocument> , int )>
    {
        private readonly IDocumentRepository _documentRepository;
        public GetAllDocumentQueryHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }
        public  Task<(IEnumerable<MasterDocument> , int )> Handle(GetAllDocumentQuery request, CancellationToken cancellationToken)
        {

            return _documentRepository.GetAllDocuments(request.pageNumber, request.pageSize);
        }
    }
}
