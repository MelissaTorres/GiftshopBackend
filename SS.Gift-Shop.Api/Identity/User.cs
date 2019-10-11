using System;
using Microsoft.AspNetCore.Identity;
using SS.GiftShop.Domain.Model;

namespace SS.GiftShop.Api.Identity
{
    public class User : IdentityUser<Guid>, IStatus<EnabledStatus>
    {
        public EnabledStatus Status { get; set; }
    }
}
