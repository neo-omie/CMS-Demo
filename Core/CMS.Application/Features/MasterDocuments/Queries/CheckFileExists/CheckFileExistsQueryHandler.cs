using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.MasterDocuments.Queries.CheckFileExists
{
    public class CheckFileExistsQueryHandler : IRequestHandler<CheckFileNameExistsQuery, bool>
    {
        private readonly IDocumentRepository _documentRepository;
        public CheckFileExistsQueryHandler(IDocumentRepository documentRepository)
        {
           _documentRepository = documentRepository;
        }
        public Task<bool> Handle(CheckFileNameExistsQuery request, CancellationToken cancellationToken)
        {
            return _documentRepository.CheckFileExists( request.documentForm);
        }
    }
}
