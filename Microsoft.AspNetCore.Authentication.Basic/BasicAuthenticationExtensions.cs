using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Basic;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BasicAuthenticationExtensions
    {
        public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder)
            => builder.AddBasic(BasicAuthenticationDefaults.AuthenticationScheme);

        public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder, string authenticationScheme)
            => builder.AddBasic(authenticationScheme, configureOptions: null);

        public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder, Action<BasicAuthenticationOptions> configureOptions)
            => builder.AddBasic(BasicAuthenticationDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder, string authenticationScheme, Action<BasicAuthenticationOptions> configureOptions)
            => builder.AddBasic(authenticationScheme, displayName: null, configureOptions: configureOptions);

        public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<BasicAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}
