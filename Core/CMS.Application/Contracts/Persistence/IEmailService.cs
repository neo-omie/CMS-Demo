using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.DTOs;

namespace CMS.Application.Contracts.Persistence
{
    public interface IEmailService
    {
        Task SendEmail(MailRequest mailRequest);
    }
}
