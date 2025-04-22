using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Application.Features.MasterDocuments
{
    public class DocumentResponse
    {
        public IEnumerable<MasterDocument> Documents { get; set; }
        public int TotalCount { get; set; }
    }
}
