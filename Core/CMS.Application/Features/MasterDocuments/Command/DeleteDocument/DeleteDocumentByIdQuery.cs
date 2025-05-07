using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CMS.Application.Features.MasterDocuments.Command.DeleteDocument
{
    public record DeleteDocumentByIdQuery (int id): IRequest<bool>;
    
}
