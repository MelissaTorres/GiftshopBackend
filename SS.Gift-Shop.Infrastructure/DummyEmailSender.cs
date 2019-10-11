using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SS.GiftShop.Core;

namespace SS.GiftShop.Infrastructure
{
    public sealed class DummyEmailSender : IEmailSender
    {
        public string Directory { get; }

        public DummyEmailSender(string directory)
        {
            Directory = directory ?? throw new ArgumentNullException(nameof(directory));
        }

        public Task Send(MailMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (!System.IO.Directory.Exists(Directory))
            {
                System.IO.Directory.CreateDirectory(Directory);
                //throw new DirectoryNotFoundException();
            }

            var timeStamp = string.Format("{0:yy}{0:MM}{0:dd}{0:HH}{0:mm}{0:ss}", DateTime.Now);
            var temp = Path.GetFileName(Path.GetTempFileName());
            var fileName = $"{timeStamp}-{temp}.html";
            using (var file = File.Create(Path.Combine(Directory, fileName)))
            {
                using (var streamWriter = new StreamWriter(file, Encoding.UTF8))
                {
                    if (!message.IsBodyHtml)
                    {
                        streamWriter.Write("<pre>");
                    }
                    streamWriter.WriteLine("From: {0}", message.From);
                    streamWriter.WriteLine("To: {0}", message.To);
                    streamWriter.WriteLine(message.Body);
                    if (!message.IsBodyHtml)
                    {
                        streamWriter.Write("</pre>");
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
