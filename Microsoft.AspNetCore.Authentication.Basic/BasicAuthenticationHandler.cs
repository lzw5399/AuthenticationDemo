using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Basic.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Microsoft.AspNetCore.Authentication.Basic
{
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private const string _Scheme = "Basic";

        public BasicAuthenticationHandler(IOptionsMonitor<BasicAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected new BasicAuthenticationEvents Events
        {
            get { return (BasicAuthenticationEvents)base.Events; }
            set { base.Events = value; }
        }

        protected override Task<object> CreateEventsAsync()=> Task.FromResult<object>(new BasicAuthenticationEvents());

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string authorizationHeader = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return  AuthenticateResult.NoResult();
            }

            if (!authorizationHeader.StartsWith(_Scheme + ' ', StringComparison.OrdinalIgnoreCase))
            {
                return  AuthenticateResult.NoResult();
            }

            string encodedCredentials = authorizationHeader.Substring(_Scheme.Length).Trim();

            if (string.IsNullOrEmpty(encodedCredentials))
            {
                return AuthenticateResult.Fail("No credentials");
            }

            string decodeedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));

            var delimiterIndex = decodeedCredentials.IndexOf(':');

            if (delimiterIndex == -1)
            {
                return AuthenticateResult.Fail("Invalid credentials, missing delimiter.");
            }

            var username = decodeedCredentials.Substring(0, delimiterIndex);
            var password = decodeedCredentials.Substring(delimiterIndex + 1);

            var validateCredentialsContext = new ValidateCredentialsContext(Context, Scheme, Options)
            {
                UserName = username,
                Password = password
            };

            await Events.ValidateCredentials(validateCredentialsContext);

            if (validateCredentialsContext.Result!=null&& validateCredentialsContext.Result.Succeeded)
            {
                var ticket = new AuthenticationTicket(validateCredentialsContext.Principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }

            //if (username == Options.UserName && password == Options.Password)
            //{
            //    Claim userName = new Claim(ClaimTypes.Name, username);

            //    ClaimsIdentity identity = new ClaimsIdentity(BasicAuthenticationDefaults.AuthenticationScheme);
            //    identity.AddClaim(userName);

            //    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            //    AuthenticationTicket ticket = new AuthenticationTicket(principal, BasicAuthenticationDefaults.AuthenticationScheme);

            //    return Task.FromResult(AuthenticateResult.Success(ticket));
            //}
            //else
            //{
            //    return Task.FromResult(AuthenticateResult.Fail("Invalid UserNam Or Password."));
            //}

            return AuthenticateResult.NoResult();
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;
            var headerValue = _Scheme + $" realm=\"{Options.Realm}\"";

            Response.Headers.Append(HeaderNames.WWWAuthenticate, headerValue);

            return Task.CompletedTask;
        }
    }
}
