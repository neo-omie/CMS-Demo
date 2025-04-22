using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Entities.CompanyMaster
{
    public class ListOfCountries
    {
        [Key]
        public int Id { get; set; }

        public string Countries { get; set; }
    }
}
