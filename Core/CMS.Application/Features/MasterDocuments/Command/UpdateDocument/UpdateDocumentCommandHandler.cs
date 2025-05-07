
using AutoMapper;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Exceptions;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterDocuments.Command.UpdateDocument
{
    public class UpdateDocumentCommandHandler : IRequestHandler<UpdateDocumentCommand, object>
    {
        private readonly IDocumentRepository _repository;
        private readonly IMapper _mapper;
        public UpdateDocumentCommandHandler(IDocumentRepository documentRepository, IMapper mapper)
        {

            _repository = documentRepository;
            _mapper = mapper;
        }
        public async  Task<object> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = await _repository.GetDocumentById(request.id);
            if (document == null)
            {
                throw new DocumentNotFoundException("Document not found");
            }
            //var existingDocument = _mapper.Map<MasterDocument>(request.documentDTO);
            //existingDocument.ValueId = request.id;
            return await _repository.UpdateDocument(request.id, request.model);

        }
    }
}
