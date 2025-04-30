using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Entities
{
    public class MasterApostille
    {
        [Key]
        public int ValueId { get; set; }
        public string ApostilleName { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
