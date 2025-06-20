using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Entities.CompanyMaster
{
    public class ListOfLocation
    {
        [Key]
        public int LocationId { get; set; }

        public string  LocationName { get; set; }

        public int CityId { get; set; }
        public ListofCity ListOfCities { get; set; }

    }
}
