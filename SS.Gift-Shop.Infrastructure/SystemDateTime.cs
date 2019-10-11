using System;
using SS.GiftShop.Core;

namespace SS.GiftShop.Infrastructure
{
    public sealed class SystemDateTime : IDateTime
    {
        private readonly TimeZoneInfo _timeZoneInfo;

        public SystemDateTime()
            : this(TimeZoneInfo.Local)
        {
        }

        public SystemDateTime(string localTimeZone)
        {
            if (localTimeZone is null)
            {
                throw new ArgumentNullException(nameof(localTimeZone));
            }

            _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(localTimeZone);
        }

        public SystemDateTime(TimeZoneInfo localTimeZoneInfo)
        {
            _timeZoneInfo = localTimeZoneInfo ?? throw new ArgumentNullException(nameof(localTimeZoneInfo));
        }

        public DateTime Now { get { return DateTime.UtcNow; } }

        public DateTime Today
        {
            get
            {
                return FromUtc(DateTime.UtcNow).Date;
            }
        }

        public DateTime FromUtc(DateTime date)
        {
            var local = TimeZoneInfo.ConvertTimeFromUtc(date, _timeZoneInfo);
            return local;
        }
    }
}
