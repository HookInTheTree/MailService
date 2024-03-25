using MailKit.Net.Smtp;
using MailService.Services.Emails;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MailService.Infrastructure.Smtp
{
    public class MailKitClient:ISmtpClient
    {
        private readonly SmtpOptions options;

        public MailKitClient(IOptions<SmtpOptions> smtpOptions)
        {
            options = smtpOptions.Value;
        }

        public async Task SendEmailAsync(Message message, CancellationToken token = default)
        {
            var msg = new MimeMessage()
            {
                Subject = message.Subject,
                Body = new BodyBuilder { HtmlBody = message.Body }.ToMessageBody(),
            };

            foreach (var recipient in message.To)
                msg.To.Add(new MailboxAddress(recipient, recipient));

            var senderName = string.IsNullOrEmpty(message.From) ? options.Sender : message.From;
            msg.From.Add(new MailboxAddress(senderName, options.UserName));

            foreach (var recipient in message.CC)
                msg.To.Add(new MailboxAddress(recipient, recipient));

            foreach (var recipient in message.BCC)
                msg.To.Add(new MailboxAddress(recipient, recipient));


            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(options.Server, options.Port, cancellationToken: token);
                await client.AuthenticateAsync(options.UserName, options.Password, token);
                await client.SendAsync(msg, token);
                await client.DisconnectAsync(true, token);
            }
        }
    }
}
