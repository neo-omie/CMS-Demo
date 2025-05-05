using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CMS.Application.Features.MasterDocuments.Command.UploadDocument
{

    public class UploadDocumentCommand : IRequest<MasterDocument>
    {
        public string DocumentName { get; set; }
        public string Status { get; set; }
        public IFormFile File { get; set; }
    }
}
