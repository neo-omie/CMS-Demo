using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.Document;
using MediatR;

namespace CMS.Application.Features.MasterDocuments.Command.UpdateDocument
{
    public record UpdateDocumentCommand(int id, DocumentDTO documentDTO) : IRequest<int>;


}
