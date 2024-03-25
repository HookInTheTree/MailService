using MimeKit;

namespace MailService.Services.Emails
{
    public class Message
    {
        public string Subject { get; private set; }
        // Получатели письма
        public List<string> To { get; private set; }

        public string? Body { get; private set; }
        public string From { get; private set; }

        // Открытые получатели письма. Видят все получатели
        public List<string> CC { get; private set; }

        // Скрытые получатели письма. Не видит никто.
        public List<string> BCC { get; private set; }

        public Message(
            List<string> to,
            string subject,
            string body = null,
            string from = null,
            List<string> cc = null,
            List<string> bcc = null)
        {
            if (to == null || to.Count == 0)
            {
                throw new ArgumentException("To can't be null or empty.");
            }
            To = to;

            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentException("Subject cant' be null or empty");
            }
            Subject = subject;

            Body = body;
            From = from;
            
            CC = cc ?? new List<string>();
            BCC = bcc ?? new List<string>();
        }
    }
}