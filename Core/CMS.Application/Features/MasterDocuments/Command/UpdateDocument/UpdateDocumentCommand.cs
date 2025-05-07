using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterDocuments.Command.UpdateDocument
{
    public record UpdateDocumentCommand(int id, DocumentFormDTO model) : IRequest<bool>;


}
