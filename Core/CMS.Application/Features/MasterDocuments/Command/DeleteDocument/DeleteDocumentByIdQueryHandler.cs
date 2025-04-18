using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.MasterDocuments.Command.DeleteDocument
{
    public class DeleteDocumentByIdQueryHandler : IRequestHandler<DeleteDocumentByIdQuery, int>
    {
        private readonly IDocumentRepository _repository;

        public DeleteDocumentByIdQueryHandler(IDocumentRepository repository)
        {
            _repository = repository;
        }
        public Task<int> Handle(DeleteDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            return _repository.DeleteDocument(request.id);

        }
    }
}
