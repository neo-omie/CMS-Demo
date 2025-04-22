using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterDocuments.Queries.GetAllDocument
{
    public record GetAllDocumentQuery(int pageNumber, int pageSize) :IRequest<(IEnumerable<MasterDocument>, int)>;
   
    
}
