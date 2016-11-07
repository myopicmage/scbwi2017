using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using scbwi2017.Models.Data;

namespace scbwi2017.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public AuthMessageSender(IOptions<Secrets> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public Secrets Options { get; } //set only via Secret Manager
        private readonly EmailAddress support_email = new EmailAddress("florida-ra@scbwi.org", "Florida RA");

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.FromResult(0);
        }

        public Task<HttpResponseMessage> SendEmailAsync(string email, string subject, string message, string name)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BEARER", Options.sendgridkey);

            var payload = new SendGridEmail
            {
                content = new List<EmailMessage>
                {
                    new EmailMessage("text/html", message)
                },
                personalizations = new List<Personalization>
                {
                    new Personalization
                    {
                        to = new [] { new EmailAddress(email, name) },
                        bcc = new []
                        {
                            new EmailAddress("florida-ra@scbwi.org", "Florida RA"),
                            new EmailAddress("kcbernfeld@gmail.com", "Kevin Bernfeld"), 
                        },
                        subject = subject
                    }
                },
                from = new EmailAddress("register@scbwiflorida.com", "SCBWI Florida Registration Bot"),
                reply_to = support_email
            };

            return client.PostAsJsonAsync("https://api.sendgrid.com/v3/mail/send", payload);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
