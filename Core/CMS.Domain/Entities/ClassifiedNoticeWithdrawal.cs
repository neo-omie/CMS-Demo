using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Entities
{
    public class ClassifiedNoticeWithdrawal
    {
        [Key]
        public int ValueId { get; set; }

        [Required]
        public int ClassifiedContractId { get; set; }
        public ClassifiedContract ClassifiedContract { get; set; }

        [Required]
        public int TerminationNoticeId { get; set; }
        public ClassifiedPostTerminationNotice ClassifiedPostTermination { get; set; }

        [Required]
        public string DisplayDocumentName { get; set; }

        [Required]
        public string DocumentPath { get; set; }

        [Required]
        public string Remark { get; set; }
    }
}
