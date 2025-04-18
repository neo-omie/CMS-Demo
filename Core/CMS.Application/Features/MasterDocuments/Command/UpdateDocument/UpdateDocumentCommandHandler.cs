
using AutoMapper;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterDocuments.Command.UpdateDocument
{
    public class UpdateDocumentCommandHandler : IRequestHandler<UpdateDocumentCommand, int>
    {
        private readonly IDocumentRepository _repository;
        private readonly IMapper _mapper;
        public UpdateDocumentCommandHandler(IDocumentRepository documentRepository, IMapper mapper)
        {

            _repository = documentRepository;
            _mapper = mapper;
        }
        public Task<int> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = _repository.GetDocumentById(request.id);
            if (document == null)
            {
                //throw new DocumentNotFound($"Document not found");
            }
            var existingDocument = _mapper.Map<MasterDocument>(request.documentDTO);
            existingDocument.ValueId = request.id;
            return _repository.UpdateDocument(existingDocument);

        }
    }
}
