using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace AuthenticationTypes.AuthHandlers
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) 
            :base(options,logger,encoder,clock)
        {
              
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing Authentication Headers"));
            }

            try {

                var authHeader = Request.Headers["Authorization"].ToString();
                var endocdedCreds = authHeader.Substring("Basic".Length).Trim();
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(endocdedCreds)).Split(':');
                var username = credentials[0];
                var password = credentials[1];

                if (username == "user" && password == "pass")
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, username) };
                    var claimIdentity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(claimIdentity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }
                else {
                    return Task.FromResult(AuthenticateResult.Fail("Invlaid Username or password"));
                }

            }
            catch (Exception ex) {
                return Task.FromResult(AuthenticateResult.Fail("Invlaid Authentication"));
            }
        }
    }
}
