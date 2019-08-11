using IdentityModel.Client;
using System;
using System.Net.Http;

namespace IdentityServer4.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            RequestAccessTokenByResourceOwnerPassword();
        }

        /// <summary>
        /// 这个方法用于使用客户端授权码模式，来获取token
        /// </summary>
        private static void RequestAccessTokenByClientCredentials()
        {
            // Client只需要知道Ids4的地址，就可以通过以下方式获取各个终结点[EndPoint]
            var client = new HttpClient();
            var disco = client.GetDiscoveryDocumentAsync("http://localhost:5000").Result;
            if (disco.IsError)
            {
                Console.WriteLine("请求EndPoint失败，原因：");
                Console.WriteLine(disco.Error);
                return;
            }

            // 接下来用从 IdentityServer MetaData获取到的Token EndPoint请求AccessToken
            var requestData = new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            };
            var tokenResponse = client.RequestClientCredentialsTokenAsync(requestData).Result;

            if (tokenResponse.IsError)
            {
                Console.WriteLine("请求AccessToken失败，原因：");
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine("通过ClientCredentials请求Token成功，如下:");
            Console.WriteLine(tokenResponse.Json);
        }

        /// <summary>
        /// 这个方法用于使用密码模式，来获取token
        /// </summary>
        private static void RequestAccessTokenByResourceOwnerPassword()
        {
            // Client只需要知道Ids4的地址，就可以通过以下方式获取各个终结点[EndPoint]
            var client = new HttpClient();
            var disco = client.GetDiscoveryDocumentAsync("http://localhost:5000").Result;
            if (disco.IsError)
            {
                Console.WriteLine("请求EndPoint失败，原因：");
                Console.WriteLine(disco.Error);
                return;
            }

            // 接下来用从 IdentityServer MetaData获取到的Token EndPoint请求AccessToken
            var requestData = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "ro.client",
                ClientSecret = "secret",

                UserName = "alice",
                Password = "password",
                Scope = "api1"
            };
            var tokenResponse = client.RequestPasswordTokenAsync(requestData).Result;

            if (tokenResponse.IsError)
            {
                Console.WriteLine("请求AccessToken失败，原因：");
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine("通过ResourceOwnerPassword请求Token成功，如下:");
            Console.WriteLine(tokenResponse.Json);
        }
    }
}
