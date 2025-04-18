using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Constants;

namespace CMS.Domain.Entities
{
    public class MasterDocument
    {

        public int ValueId { get; set; }
        public string DocumentName { get; set; }
        public Status status { get; set; } = Status.Active;

        public bool IsDeleted { get; set; } = false;
       
    }

    
}
