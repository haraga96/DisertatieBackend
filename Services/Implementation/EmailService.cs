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

        public async Task SendEmailAsync(string toPerson, string subject)
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
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync(_configuration["Gmail:Username"], _configuration["Gmail:Password"]);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        public async Task SendComputeAsync(string toPerson, string subject, int sum)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Tax App", _configuration["ResetPassword:From"]));
            message.To.Add(new MailboxAddress(new MailboxAddress(toPerson).Name, toPerson));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = string.Format("You have to pay {0} lei", sum)
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync(_configuration["Gmail:Username"], _configuration["Gmail:Password"]);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}