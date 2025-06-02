using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.Notifications.Queries.GetAllNotifications
{
    public class GetAllNotificationsDto
    {

        public int ValueId { get; set; }
        public string NotficationSubject { get; set; }
        public bool IsRead { get; set; }
        public DateTime NotificationDate { get; set; }
        public int totalRecords { get; set; }
    }
}
