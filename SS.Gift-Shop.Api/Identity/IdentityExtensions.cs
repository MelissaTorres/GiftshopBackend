using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace SS.GiftShop.Api.Identity
{
    public static class IdentityExtensions
    {
        private static readonly string[] Empty = new string[0];

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime? GetExpiration(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            var value = GetValue(identity, "exp");
            if (value != null)
            {
                if (long.TryParse(value, out var exp))
                {
                    var time = TimeSpan.FromSeconds(exp);
                    return Epoch.Add(time);
                }
            }

            return default;
        }

        public static IEnumerable<string> GetRoles(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            return GetValues(identity, ClaimTypes.Role);
        }

        public static Guid GetUserId(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            var value = GetValue(identity, ClaimTypes.NameIdentifier);
            if (value != null)
            {
                if (Guid.TryParse(value, out var userId))
                {
                    return userId;
                }
            }

            return default;
        }

        public static bool IsInRole(this IIdentity identity, string role)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return GetRoles(identity).Any(x => string.Equals(x, role, StringComparison.OrdinalIgnoreCase));
        }

        private static string GetValue(IIdentity identity, string key)
        {
            return GetValues(identity, key).FirstOrDefault();
        }

        private static IEnumerable<string> GetValues(IIdentity identity, string key)
        {
            if (identity is ClaimsIdentity claimsIdentity)
            {
                return claimsIdentity.FindAll(key)
                    .Select(x => x.Value);
            }

            return Empty;
        }
    }
}
