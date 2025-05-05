using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.DTOs;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CMS.Persistence.Repositories
{
    public class EmailService : IEmailService
    {
        readonly EmailSettings _settings;
        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task SendEmail(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_settings.Email);
            email.To.Add(MailboxAddress.Parse(mailRequest.Email));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = mailRequest.EmailBody;
            email.Body = builder.ToMessageBody();
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_settings.Host, _settings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_settings.Email, _settings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
            //var fromAddess = new MailAddress(_settings.Email, "CMS");
            //var smtp = new SmtpClient
            //{
            //    Host = _settings.Host,
            //    Port = _settings.Port,
            //    EnableSsl = true,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    UseDefaultCredentials = false,
            //    Credentials = new NetworkCredential(_settings.Email, _settings.Password)
            //};
            //using (var msg = new MailMessage(_settings.Email, mailRequest.Email)
            //{
            //    Subject = mailRequest.Subject,
            //    Body = mailRequest.EmailBody
            //}) { smtp.Send(msg); }
        }
    }
}
