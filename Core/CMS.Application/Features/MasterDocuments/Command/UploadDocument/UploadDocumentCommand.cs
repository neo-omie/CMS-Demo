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

    public record UploadDocumentCommand(DocumentUploadDto model) : IRequest<string>;
}
