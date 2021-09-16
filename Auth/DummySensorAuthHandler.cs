using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Overwatcher.Auth
{
    public class DummySensorAuthHandler : AuthenticationHandler<DummySensorAuthOptions>
    {
        public const string AuthenticationScheme = "dummy";
        
        public DummySensorAuthHandler(IOptionsMonitor<DummySensorAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

#pragma warning disable 1998
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
#pragma warning restore 1998
        {
            var token = "";
            var authorization = Request.Headers[HeaderNames.Authorization].ToString();

            // If no authorization header found, nothing to process further
            if (string.IsNullOrEmpty(authorization))
                return AuthenticateResult.NoResult();

            if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                token = authorization.Substring("Bearer ".Length).Trim();

            // If no token found, no further work possible
            if (string.IsNullOrEmpty(token))
                return AuthenticateResult.NoResult();

            var claimsPrincipal = new ClaimsPrincipal(new []
            {
                new ClaimsIdentity(
                    new []
                    {
                        new Claim("sensorId", token)
                    }, "Dummy")
            });

            return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var authResult = await HandleAuthenticateOnceSafeAsync();

            if (authResult.Succeeded)
                return;

            Response.StatusCode = 401;
        }
    }
}