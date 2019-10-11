using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SS.GiftShop.Core;

namespace SS.GiftShop.Infrastructure
{
    public sealed class SmtpEmailSender : IEmailSender
    {
        public const int DefaultTimeout = 100000;
        private readonly SmtpSettings _smtpSettings;

        /// <summary>
        /// Gets or sets a value that specifies the amount of time after which a synchronous <see cref="SmtpClient.Send"/> call times out.
        /// </summary>
        /// <value>
        /// An System.Int32 that specifies the time-out value in milliseconds. The default value is 100,000 (100 seconds).
        /// </value>
        public int? Timeout { get; set; }

        public SmtpEmailSender(IOptions<SmtpSettings> smtpSettingsOptions)
        {
            _smtpSettings = smtpSettingsOptions.Value;
        }

        public async Task Send(MailMessage message)
        {
            using (var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
            {
                client.UseDefaultCredentials = false;
                client.EnableSsl = _smtpSettings.EnableSsl;
                client.Credentials = new NetworkCredential(_smtpSettings.User, _smtpSettings.Password);

                if (_smtpSettings.Timeout.HasValue)
                {
                    client.Timeout = _smtpSettings.Timeout.Value;
                }

                if (message.From == null)
                {
                    message.From = new MailAddress(_smtpSettings.From, _smtpSettings.FromDisplay);
                }

                await client.SendMailAsync(message);
            }
        }
    }
}
