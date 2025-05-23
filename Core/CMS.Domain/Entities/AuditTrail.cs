using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Constants;

namespace CMS.Domain.Entities
{
    public class AuditTrail
    {
        [Key]
        public int ValueId { get; set; }
        public int TableId { get; set; }
        public TableList ForTable { get; set; }
        public string ActionDescription { get; set; }
        public DateTime LogTime { get; set; }
        public string LoggedBy { get; set; } // FK of Employee Code from MasterEmployee Table.
        public MasterEmployee Employee { get; set; }
        public LogStatus Status { get; set; }
    }
}
