using IdentityDemo.Data;
using IdentityDemo.Models;
using IdentityDemo.Services;
using Microsoft.AspNetCore.Authentication.QQ;
using Microsoft.AspNetCore.Authentication.WeChat;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace IdentityDemo
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc();

            // Identity配置
            /*services
                .AddIdentityCore<ApplicationUser>(a =>
                    {
                        a.Lockout = new LockoutOptions
                        {
                            AllowedForNewUsers = true,
                            DefaultLockoutTimeSpan = TimeSpan.FromSeconds(20),
                            MaxFailedAccessAttempts = 3
                        };
                        a.Password = new PasswordOptions
                        {
                            RequireDigit = true,
                            RequiredLength = 20,
                            RequiredUniqueChars = 5,
                            RequireLowercase = true,
                            RequireNonAlphanumeric = false,
                            RequireUppercase = false
                        };
                        a.SignIn = new SignInOptions
                        {
                            RequireConfirmedEmail = false,
                            RequireConfirmedPhoneNumber = false
                        };
                        a.Stores = new StoreOptions
                        {
                            MaxLengthForKeys = 20,
                            ProtectPersonalData = true
                        };
                        a.Tokens = new TokenOptions
                        {
                            AuthenticatorIssuer = "23",
                            AuthenticatorTokenProvider = "23",
                            ChangeEmailTokenProvider = "23",
                            ChangePhoneNumberTokenProvider = "23",
                            EmailConfirmationTokenProvider = "23",
                            PasswordResetTokenProvider = "23",
                            ProviderMap = new Dictionary<string, TokenProviderDescriptor>()
                        };
                        a.User = new UserOptions
                        {
                            AllowedUserNameCharacters = "23",
                            RequireUniqueEmail = true
                        };
                        a.ClaimsIdentity = new ClaimsIdentityOptions
                        {
                            RoleClaimType = "23",
                            SecurityStampClaimType = "3",
                            UserIdClaimType = "23",
                            UserNameClaimType = "23"
                        };
                    })*/

            // Identity Cookie配置
            //services.ConfigureApplicationCookie(a =>
            //{
            //    a.AccessDeniedPath = "/home/accessDenied/";
            //    a.Cookie.Name = "cookieName";
            //    a.Cookie.HttpOnly = true; // 设置js无法查看cookie
            //    a.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            //    a.LoginPath = "/user/login"; // 登录界面可定制
            //    a.ReturnUrlParameter = "return"; // 默认是returnUrl
            //    a.SlidingExpiration = true; // 滑动过期
            //});

            services
                .AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddMicrosoftAccount(a =>
            {
                a.ClientId = "id";
                a.ClientSecret = "password";
            })
            .AddGoogle(g =>
            {
                g.ClientId = "id";
                g.ClientSecret = "sec";
            })
            .AddTwitter(t =>
            {
                t.ConsumerKey = "23";
                t.ConsumerSecret = "233";
            })
            .AddQQ(q =>
            {
                q.AppId = "23";
                q.AppKey = "23";
            })
            .AddWeChat(w =>
            {
                w.AppId = "23";
                w.AppSecret = "233";
            })
            .AddIdentityCookies(o => { });

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services
                .Configure<MailSenderOptions>(Configuration.GetSection("MailSender"))
                .Configure<SmsSenderOptions>(Configuration.GetSection("SmsSender"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}