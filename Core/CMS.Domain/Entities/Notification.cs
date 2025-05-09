using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    }
}
