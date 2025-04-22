using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Constants;

namespace CMS.Application.Features.Document
{
    public class DocumentDTO
    {
        public string DoccumentName { get; set; }

        public Status status { get; set; }
    }
}
