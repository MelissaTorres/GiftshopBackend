using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SS.GiftShop.Core;

namespace SS.GiftShop.Infrastructure
{
    public sealed class SmtpSettings : ISettings
    {
        private static readonly EmailAddressAttribute EmailAddres = new EmailAddressAttribute();
        public string Host { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string From { get; set; }

        public string FromDisplay { get; set; }

        public int Port { get; set; } = 587;

        public bool EnableSsl { get; set; } = true;

        public int? Timeout { get; set; }

        public void Validate()
        {
            var errors = new List<string>();
            var requiredProps = new Dictionary<string, string>
            {
                { nameof(Host), Host },
                { nameof(User), User },
                { nameof(Password), Password },
                { nameof(From), From },
            };

            foreach (var item in requiredProps)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    errors.Add($"{item.Key} is required.");
                }
            }

            if (string.IsNullOrEmpty(From) && EmailAddres.IsValid(From))
            {
                errors.Add($"{nameof(From)} address {From} is not a valid email address.");
            }

            if (Timeout.HasValue && Timeout.Value <= 0)
            {
                errors.Add($"{nameof(Timeout)} value {Timeout} is out of range.");
            }

            if (errors.Any())
            {
                throw new SettingsException(string.Join(Environment.NewLine, errors));
            }
        }
    }
}
