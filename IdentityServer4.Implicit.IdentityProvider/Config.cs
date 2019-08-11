using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Implicit.IdentityProvider
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
               // OpenID Connect implicit flow client (MVC)
               new Client
               {
                   ClientId = "mvc",
                   ClientName = "MVC Client",
                   AllowedGrantTypes = GrantTypes.Implicit,

                   // 登录成功回调处理地址，处理回调返回的数据
                   // 当前这个路径是AddOpenIdConnect默认的
                   // 如果想要修改，去API那边修改options.CallbackPath
                   RedirectUris = { "http://localhost:5002/signin-oidc" },

                   // 注销之后的回调地址
                   // 当前这个路径是AddOpenIdConnect默认的
                   // 如果想要修改，去API那边修改options.SignedOutCallbackPath
                   PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                   AllowedScopes = new List<string>
                   {
                       IdentityServerConstants.StandardScopes.OpenId,
                       IdentityServerConstants.StandardScopes.Profile
                   },
                   // 是否显示授权界面，默认开启
                   RequireConsent = true
                }
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password",

                    // OpenID Connect处理程序默认要求 profile scope。此Scope还包括 name 或者 website
                    // 将这些Claim添加到用户信息中，以便IdentityServer可以将它们放入 identity token
                    Claims = new []
                    {
                        new Claim("name", "Alice"),
                        new Claim("website", "https://alice.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password"
                }
            };
        }
    }
}