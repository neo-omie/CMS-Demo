using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Constants;

namespace CMS.Application.Features.AuditTrails.Queries.GetAllAudits
{
    public class GetAllAuditDto
    {
        [NotMapped]
        public string TableName { get; set; } 
       public TableList ForTable { get; set; }

        public string ActionDescription { get; set; }

        public DateTime LogTime { get; set; }

        public string LoggedBy { get; set; }
        [NotMapped]
        public string StatusName { get; set; }
        public LogStatus Status { get; set; }

        public int TotalRecords { get; set; }
    }
}
