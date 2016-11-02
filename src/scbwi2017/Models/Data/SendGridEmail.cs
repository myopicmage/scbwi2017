using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scbwi2017.Models.Data
{
    public class SendGridEmail
    {
        public IEnumerable<Personalization> personalizations { get; set; }
        public EmailAddress from { get; set; }
        public EmailAddress reply_to { get; set; }
        public IEnumerable<EmailMessage> content { get; set; }
        //public string template_id { get; set; }
    }

    public class EmailAddress
    {
        public string email { get; set; }
        public string name { get; set; }

        public EmailAddress()
        {
        }

        public EmailAddress(string address, string name)
        {
            email = address;
            this.name = name;
        }
    }

    public class Personalization
    {
        public IEnumerable<EmailAddress> to { get; set; }
        //public IEnumerable<EmailAddress> cc { get; set; }
        public IEnumerable<EmailAddress> bcc { get; set; }
        public string subject { get; set; }
        //public IEnumerable<object> substitutions { get; set; }
    }

    public class EmailMessage
    {
        public string type { get; set; }
        public string value { get; set; }

        public EmailMessage() { }

        public EmailMessage(string t, string v)
        {
            type = t;
            value = v;
        }
    }
}
