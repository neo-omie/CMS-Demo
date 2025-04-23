using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Entities.CompanyMaster
{
   public class ListofCity
    {
        [Key]
        public int CityId { get; set; }

        public string City { get; set; }

        public int StateId { get; set; }
        public ListOfStates listofStates { get; set; }

    }
}
