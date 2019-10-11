using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace SS.GiftShop.Api.Identity
{
    public class SecretBearerAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string Secret { get; set; }
    }

    public class SecretBearerAuthenticationHandler : AuthenticationHandler<SecretBearerAuthenticationOptions>
    {
        public SecretBearerAuthenticationHandler(IOptionsMonitor<SecretBearerAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string token = null;
            string authorization = Request.Headers[HeaderNames.Authorization];
            // If no authorization header found, nothing to process further
            if (string.IsNullOrEmpty(authorization))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = authorization.Substring("Bearer ".Length).Trim();
            }

            // If no token found, no further work possible
            if (string.IsNullOrEmpty(token))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            if (token == Options.Secret)
            {
                var identity = new ClaimsIdentity(Options.ClaimsIssuer);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "1"));
                identity.AddClaim(new Claim(ClaimTypes.Name, Role.Terminal));
                identity.AddClaim(new Claim(ClaimTypes.Role, Role.Terminal));
                var principal = new ClaimsPrincipal(identity);
                return Task.FromResult(
                    AuthenticateResult.Success(new AuthenticationTicket(principal, Options.ClaimsIssuer)));
            }

            return Task.FromResult(AuthenticateResult.Fail("Bad token"));
        }
    }
}
