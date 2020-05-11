using System;
using System.Threading.Tasks;
using Backend_Dis_App.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Backend_Dis_App.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string toPerson, string subject)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Tax App", _configuration["ResetPassword:From"]));
            message.To.Add(new MailboxAddress(new MailboxAddress(toPerson).Name, toPerson));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = _configuration["ResetPassword:Url"]
            };

            using (var client = new SmtpClient())
            {

                client.Connect("smtp.gmail.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(_configuration["Gmail:Username"], _configuration["Gmail:Password"]);

                client.Send(message);
                client.Disconnect(true);

            }

        }
    }
}