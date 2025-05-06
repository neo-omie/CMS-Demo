using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Constants;
using CMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CMS.Application.Features.MasterDocuments.Command.UploadDocument
{
    public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, string>
    {
        private readonly IDocumentRepository _documentRepository;
        public UploadDocumentCommandHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<string> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {
            return await _documentRepository.UploadDocument(request.model);
        }
    }

}