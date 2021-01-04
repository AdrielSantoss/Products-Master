using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace API.Authentication
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string ApiKey = "ed63ffc4-3237-4839-9ced-01fdc91297df";
        public const string SchemeName = "ApiKey";

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.Headers.TryGetValue("Authorization", out StringValues value))
            {
                var scheme = SchemeName + " ";
                var header = value.FirstOrDefault();
                if (header?.StartsWith(scheme, StringComparison.OrdinalIgnoreCase) != true)
                {
                    return Task.FromResult(AuthenticateResult.NoResult());
                }
                var apiKey = header.Substring(scheme.Length);
                if (apiKey == ApiKey)
                {
                    return Task.FromResult(AuthenticateResult.Success(CreateAuthenticationTicket()));
                }
                return Task.FromResult(AuthenticateResult.Fail("A api-key especificada não é válida."));
            }
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        private AuthenticationTicket CreateAuthenticationTicket()
        {
            var claims = new[] { new Claim(ClaimTypes.Name, SchemeName) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            return new AuthenticationTicket(principal, Scheme.Name);
        }
    }
}

