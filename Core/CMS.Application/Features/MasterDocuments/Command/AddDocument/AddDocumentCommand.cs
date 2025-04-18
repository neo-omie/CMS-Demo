using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.Document;
using MediatR;

namespace CMS.Application.Features.MasterDocuments.Command.AddDocument
{
    public record AddDocumentCommand(DocumentDTO documentDTO) : IRequest<int>;

}
