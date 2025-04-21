using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Entities.CompanyMaster
{
   public  class ListOfStates
    {
        public int Id { get; set; }

        public string State { get; set; }

        public ListOfCountries listofcountries { get; set; }
    }
}
