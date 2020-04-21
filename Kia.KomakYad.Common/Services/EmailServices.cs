using Kia.KomakYad.Common.Configurations;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Kia.KomakYad.Common.Services
{
    public class EmailService
    {
        private EmailConfiguration _configuration;

        public EmailService(IOptions<EmailConfiguration> options)
        {
            _configuration = options.Value;
        }

        public async Task SendAsync(string body, string subject, string emailAddress, bool isHtml = true)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration.SenderEmail, _configuration.SenderEmail),
                IsBodyHtml = isHtml,
                Subject = subject,
                Body = body
            };

            mailMessage.To.Add(emailAddress);

            using (var smtp = new SmtpClient(_configuration.Host, _configuration.Port))
            {
                smtp.Credentials = new NetworkCredential(_configuration.SenderEmail, _configuration.Password);
                await smtp.SendMailAsync(mailMessage);
            }
        }
    }
}
