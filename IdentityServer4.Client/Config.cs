using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer4.Models;

namespace IdentityServer4.Client
{
    public static class Config
    {
        public static IEnumerable<Models.Client> GetClients()
        {
            return new List<Models.Client>
            {
                new Models.Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "api1" }
                }
            };
        }
    }
}
