using MailService.Infrastructure.Smtp;

namespace MailService.Services.Emails
{
    public class MailService : IMailService
    {
        private readonly ISmtpClient _client;
        public MailService(ISmtpClient client)
        {
            _client = client;
        }

        public async Task SendAsync(Message messageInfo)
        {
            await _client.SendEmailAsync(messageInfo);
        }
    }
}
