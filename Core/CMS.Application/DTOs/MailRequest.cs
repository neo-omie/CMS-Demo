using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.DTOs
{
    public class MailRequest
    {
        public string Email { get; set; }
        public string? EmailCC { get; set; }

        public string Subject { get; set; }
        public string EmailBody { get; set; }

        
    }

    public class MailRequestWithAttachment
    {
        public string Email { get; set; }
        public string? EmailCC { get; set; }

        public string Subject { get; set; }
        public string EmailBody { get; set; }
        public string Attachments { get; set; }
    }
}
