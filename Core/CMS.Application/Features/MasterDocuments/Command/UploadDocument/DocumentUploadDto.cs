using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace CMS.Application.Features.MasterDocuments.Command.UploadDocument
{
    public class DocumentUploadDto
    {
       
        public Status Status { get; set; } 
        public IFormFile File { get; set; }

        //public bool IsDeleted { get; set; } = false;

    }
}
