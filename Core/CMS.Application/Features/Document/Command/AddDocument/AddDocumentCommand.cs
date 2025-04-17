using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CMS.Application.Features.Document.Command.AddDocument
{
    public record AddDocumentCommand(DocumentDTO documentDTO):IRequest<int>;
    
}
