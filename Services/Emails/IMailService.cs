namespace MailService.Services.Emails
{
    public interface IMailService
    {
        public Task SendAsync(Message messageInfo);
    }
}
