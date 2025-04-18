using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterDocuments.Command.AddDocument
{
    public class AddDocumentCommandHandler : IRequestHandler<AddDocumentCommand, int>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IMapper _mapper;

        public AddDocumentCommandHandler(IDocumentRepository documentRepository, IMapper mapper)
        {

            _documentRepository = documentRepository;
            _mapper = mapper;
        }
        public Task<int> Handle(AddDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = _mapper.Map<MasterDocument>(request);
            return _documentRepository.AddDocument(document);
        }
    }
}
