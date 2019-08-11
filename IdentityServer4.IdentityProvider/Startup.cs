using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer4.IdentityProvider
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            #region 第一种方法，基于客户端授权码模式的配置
            // 第一种方法，基于客户端授权码模式的配置
            //var builder = services.AddIdentityServer()
            //    .AddDeveloperSigningCredential()
            //    .AddInMemoryIdentityResources(Config.GetIdentityResources())
            //    .AddInMemoryApiResources(Config.GetApis())
            //    .AddInMemoryClients(Config.GetClients());
            #endregion

            #region 第二种方法，基于密码模式的配置
            // 第二种方法，基于密码模式的配置
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetUsers());
            /* AddTestUsers 方法帮我们做了以下几件事：
             * 1、为资源所有者密码授权添加支持
             * 2、添加对用户相关服务的支持，这服务通常为登录 UI 所使用（我们将在下一个快速入门中用到登录 UI）
             * 3、为基于测试用户的身份信息服务添加支持（你将在下一个快速入门中学习更多与之相关的东西）
             */
            #endregion
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseIdentityServer();
        }
    }
}
