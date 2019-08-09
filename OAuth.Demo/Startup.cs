using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace OAuth.Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            #region 默认的实现OAuth实现
            // 默认的实现OAuth实现
            //services
            //    .AddAuthentication(options =>
            //    {
            //        // 认证token
            //        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //        // cookie保存token
            //        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //        // 资询方案 这里填微软账号,那么访问[Authorize]的方法,如果没登录就会自动跳到微软登录界面
            //        options.DefaultChallengeScheme = MicrosoftAccountDefaults.AuthenticationScheme;
            //    })
            //    .AddCookie()
            //    .AddMicrosoftAccount(options =>
            //    {
            //        options.ClientId = "49737c1e-3fe6-4d57-811b-584b0a8c108a";
            //        options.ClientSecret = "FXD-yCeh3+kbw2/KVD?k4BEAo7Wu4[Lq";

            //    });
            #endregion

            #region 自己实现的通过OAuth2.0实现的
            // 自己实现的通过OAuth2.0实现的
            //services
            //    .AddAuthentication(options =>
            //    {
            //        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //        options.DefaultChallengeScheme = "MicrosoftAuth";
            //    })
            //    .AddCookie()
            //    .AddOAuth("MicrosoftAuth", options =>
            //    {
            //        options.ClientId = "49737c1e-3fe6-4d57-811b-584b0a8c108a";
            //        options.ClientSecret = "FXD-yCeh3+kbw2/KVD?k4BEAo7Wu4[Lq";
            //        // 令牌终结点
            //        options.TokenEndpoint = "https://login.microsoftonline.com/common/oauth2/v2.0/token";
            //        // 授权终结点
            //        options.AuthorizationEndpoint = "https://login.microsoftonline.com/common/oauth2/v2.0/authorize";
            //        // 用户终结点，可以通过token换取用户信息
            //        options.UserInformationEndpoint = "https://graph.microsoft.com/v1.0/me";

            //        // 我们回调的地址
            //        options.CallbackPath = new PathString("/signin-microsoft");
            //        options.Scope.Add("https://graph.microsoft.com/user.read");
            //        options.SaveTokens = true;

            //        options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            //        options.ClaimActions.MapJsonKey(ClaimTypes.Name, "displayName");
            //        options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "givenName");
            //        options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "surname");
            //        options.ClaimActions.MapCustomJson(ClaimTypes.Email, user => user.Value<string>("mail") ?? user.Value<string>("email"));

            //        // 用Event来完成获取到access_token之后再去请求获取user_info
            //        options.Events.OnCreatingTicket = async context =>
            //        {
            //            var request = new HttpRequestMessage(HttpMethod.Get, options.UserInformationEndpoint);
            //            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

            //            var response = await context.Backchannel.SendAsync(request);
            //            if (!response.IsSuccessStatusCode)
            //            {
            //                throw new Exception("xxx");
            //            }
            //            // 获取用户信息
            //            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            //            // 将用户信息绑定到【HttpContext.Identity】
            //            context.RunClaimActions(payload);
            //        };
            //    });
            #endregion

            #region 自己实现的通过OIDC实现的
            // 自己实现的通过OIDC实现的
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = "MicrosoftOIDC";
                })
                .AddCookie()
                .AddOpenIdConnect("MicrosoftOIDC", options =>
                {
                    options.ClientId = "49737c1e-3fe6-4d57-811b-584b0a8c108a";
                    options.ClientSecret = "FXD-yCeh3+kbw2/KVD?k4BEAo7Wu4[Lq";
                    options.CallbackPath = new PathString("/signin-microsoft");

                    // 元数据信息
                    options.MetadataAddress = "https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration";

                    // 请求的数据访问
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");

                    // 设置的响应类型
                    options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                    // 响应的数据怎么回来的，这里是表单提交，还可以是queryString
                    options.ResponseMode = OpenIdConnectResponseMode.FormPost;

                    // 是否把用户信息声明加到【HttpContext.User】里面去
                    options.GetClaimsFromUserInfoEndpoint = true;
                    // 设置name的claim名称，跟jwt里面的一样
                    options.TokenValidationParameters.NameClaimType = "name";

                    // 存储【access_token】和【refresh_token】，具体依方案而定(这里是cookie)
                    options.SaveTokens = true;

                    // 针对返回回来的jwt格式的【id_token】进行解密的操作
                    //options.TokenValidationParameters = new TokenValidationParameters
                    //{
                    //    ValidIssuer = "xxx"
                    //};
                });
            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
