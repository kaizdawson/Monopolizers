using Microsoft.Extensions.Options;
using Monopolizers.Common.DTO;
using Monopolizers.Common.Helpers;
using Monopolizers.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Service.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSettings _emailSettings;

        public EmailService(IEmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public async Task SendEmailAsync(EmailDTO emailDto)
        {
            var mail = new MailMessage()
            {
                From = new MailAddress(_emailSettings.Email, _emailSettings.DisplayName),
                Subject = emailDto.Subject,
                Body = emailDto.Body,
                IsBodyHtml = true
            };

            mail.To.Add(emailDto.To);

            using var smtp = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
            {
                Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);
        }
    }
}
