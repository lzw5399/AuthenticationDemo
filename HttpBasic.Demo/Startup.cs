using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Basic;
using Microsoft.AspNetCore.Authentication.Basic.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HttpBasic.Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services
                .AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
                .AddBasic(options =>
                {
                    options.Realm = "Test Realm";
                    options.Events = new BasicAuthenticationEvents
                    {
                        OnValidateCredentials = context =>
                        {
                            //访问数据库判断用户名密码
                            if (context.UserName == context.Password)
                            {
                                Claim userName = new Claim(ClaimTypes.Name, context.UserName);

                                ClaimsIdentity identity = new ClaimsIdentity(BasicAuthenticationDefaults.AuthenticationScheme);
                                identity.AddClaim(userName);

                                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                                context.Principal = principal;
                                context.Success();
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

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
