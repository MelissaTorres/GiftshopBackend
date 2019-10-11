using System;
using System.Collections.Generic;

namespace SS.GiftShop.Api.Identity.Models
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public long? ExpiresIn { get; set; }
    }
}
