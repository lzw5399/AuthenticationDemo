using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace JwtBearer.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //令牌颁发者：http://localhost:59541
            //API：http://localhost:60292

            var token = GetJwt();
            Console.WriteLine(token);

            var useResult = UseJwt(token);
            Console.WriteLine(useResult);

            Console.ReadKey();
        }

        static string GetJwt()
        {
            var client = new HttpClient();

            var user = new { UserName = "admin", Password = "admin" };

            var content = new StringContent(JsonConvert.SerializeObject(user));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = client.PostAsync("http://localhost:59541/api/token", content).Result;

            string token = "获取token失败";

            if (response.IsSuccessStatusCode)
            {
                token = response.Content.ReadAsStringAsync().Result;
            }

            return token;
        }

        static string UseJwt(string token)
        {
            var result = "使用jwt失败";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            var response = httpClient.GetAsync("http://localhost:60292/api/usetoken").Result;

            Console.WriteLine("请求API的statuscode=" + response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
            }

            return result;
        }
    }
}
