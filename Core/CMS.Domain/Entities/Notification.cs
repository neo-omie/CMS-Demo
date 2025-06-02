using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Entities
{
    public class Notification
    {
        [Key]
        public int ValueId { get; set; }
        public string EmployeeCode { get; set; }
        public string NotficationSubject { get; set; }
        public string NotficationMessage { get; set; }
        public DateTime NotificationDate { get; set; } = DateTime.Now;
        public bool isRead { get; set; } = false;
        public bool isDeleted { get; set; } = false;

        [NotMapped]
        public int totalRecords { get; set; }
    }
}
