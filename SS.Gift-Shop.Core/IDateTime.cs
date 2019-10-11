using System;

namespace SS.GiftShop.Core
{
    public interface IDateTime
    {
        DateTime Now { get; }

        DateTime Today { get; }

        DateTime FromUtc(DateTime date);
    }
}
