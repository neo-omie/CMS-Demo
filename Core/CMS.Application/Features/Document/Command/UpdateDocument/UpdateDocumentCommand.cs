using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CMS.Application.Features.Document.Command.UpdateDocument
{
    public record UpdateDocumentCommand(int id ,DocumentDTO documentDTO):IRequest<int>;
    
    
}
