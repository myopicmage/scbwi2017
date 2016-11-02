using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace scbwi2017.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task<HttpResponseMessage> SendEmailAsync(string email, string subject, string message, string name);
    }
}
