using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SS.GiftShop.Core
{
    public interface IEmailSender
    {
        Task Send(MailMessage message);
    }

    public static class EmailSenderExtensions
    {
        public static Task Send(this IEmailSender emailSender, string subject,
            string email, string body, bool isBodyHtml = true)
        {
            if (emailSender == null)
            {
                throw new ArgumentNullException(nameof(emailSender));
            }

            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            var mailMessage = CreateMailMessage(subject, email, body, isBodyHtml);

            return emailSender.Send(mailMessage);
        }

        private static MailMessage CreateMailMessage(string subject, string to, string body = null, bool isBodyHtml = true)
        {
            var mailMessage = new MailMessage();
            mailMessage.To.Add(to);
            mailMessage.Subject = subject;
            if (!string.IsNullOrEmpty(body))
            {
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = isBodyHtml;
            }
            return mailMessage;
        }
    }
}
