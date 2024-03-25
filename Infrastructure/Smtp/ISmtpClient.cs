using MailService.Services.Emails;

namespace MailService.Infrastructure.Smtp
{
    /// <summary>
    /// Контракт SMTP-сервиса
    /// </summary>
    public interface ISmtpClient
    {
        /// <summary>
        /// Отправить email
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task SendEmailAsync(Message message, CancellationToken token = default);
    }
}
