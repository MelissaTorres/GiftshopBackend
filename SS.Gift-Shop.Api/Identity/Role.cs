using System;
using Microsoft.AspNetCore.Identity;

namespace SS.GiftShop.Api.Identity
{
    public class Role : IdentityRole<Guid>
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Terminal = "Terminal";
    }
}
