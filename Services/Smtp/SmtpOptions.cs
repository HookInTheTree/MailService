using System.Net;
using System.Text;

namespace MailService.Infrastructure.Smtp
{
    public class SmtpOptions
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Sender { get;set; }
        public string Password { get; set; }
    }
}