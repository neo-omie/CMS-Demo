using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterCompanies
{
    public class GetMastersDTO
    {
        public int TotalRecords { get; set; }
        public int ValueId { get; set; }

        public string CompanyName { get; set; }

        public string CompanyLocation { get; set; }

        public bool status { get; set; } = false;
    }
}
