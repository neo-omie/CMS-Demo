using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Entities
{
    public class NoticeWithdrawal
    {
        [Key]
        public int ValueId { get; set; }

        [Required]
        public int ContractId { get; set; }
        public Contract Contract { get; set; }

        [Required]
        public int TerminationNoticeId { get; set; }
        public PostTerminationNotice PostTermination { get; set; }

        [Required]
        public string DisplayDocumentName { get; set; }

        [Required]
        public string DocumentPath { get; set; }

        [Required]
        public string Remark { get; set; }
    }
}
