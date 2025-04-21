using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Entities.CompanyMaster
{
   public class ListofCity
    {
        public int Id { get; set; }

        public string City { get; set; }

        public ListofCity listofCity { get; set; }
    }
}
