using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Entities
{
    public  class ClassifiedPostTerminationNotice
    {

        [Key]
        public int ValueId { get; set; }

        [Required]
        public int ClassifiedContractId { get; set; }
        public ClassifiedContract ClassifiedContract { get; set; }

        [Required]
        public string DisplayDocumentName { get; set; }

        [Required]
        public string DocumentPath { get; set; }



        [Required]
        public int Notice_Duration { get; set; }

        [Required]
        public DateTime End_Date { get; set; }

        [Required]
        public string Remark { get; set; }


    }
}
