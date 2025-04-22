using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Entities.CompanyMaster
{
   public  class ListOfStates
    {
        [Key]
        public int Id { get; set; }

        public string State { get; set; }

        public ListOfCountries listofcountries { get; set; }

        public int CountryId { get; set; }
    }
}
