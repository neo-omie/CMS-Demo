using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Exceptions;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterDocuments.Queries.GetDocumentById
{
    public class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, MasterDocument>
    {
        private readonly IDocumentRepository _documentRepository;
        public GetDocumentByIdQueryHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }
        public async Task<MasterDocument> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            var document = await _documentRepository.GetDocumentById(request.id);
            if (document == null)
            {
                throw new DocumentNotFoundException("Document not found");
            }
            return document;
        }
    }
}
