using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Entities
{
   public class ContractTypeMasters
    {
        [Key]
        public int ValueId { get; set; }

        public string ContractTypeName { get; set; }

        public bool Status { get; set; } = true;

        //for soft delete
        public bool IsDeleted { get; set; } = false;
    }
}
