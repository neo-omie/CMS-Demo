using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Entities.CompanyMaster
{
    public  class CompanyBranch
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int  CompanyId { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Required]
        public int StateId { get; set; }
        [Required]
        public int CityId { get; set; }
        [Required]
        public int LocationId { get; set; }
    }
}
