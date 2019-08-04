using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace JwtBearer.API
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
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // mock jwt的密钥secret
                    var secret = "Thisisthesecretkey!@#$%^&*()_+";
                    var key = Encoding.ASCII.GetBytes(secret);
                    options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(key);

                    // mocked的rsa的key
                    //var publicParameters = new RSAParameters();
                    //options.TokenValidationParameters.IssuerSigningKey = new RsaSecurityKey(publicParameters);

                    options.TokenValidationParameters.ValidateIssuer = true;
                    options.TokenValidationParameters.ValidateAudience = true;
                    options.TokenValidationParameters.ValidateIssuerSigningKey = true;
                    options.TokenValidationParameters.ValidAudience = "api";
                    options.TokenValidationParameters.ValidIssuer = "http://localhost:59541";
                    options.TokenValidationParameters.NameClaimType = JwtClaimTypes.Name;
                    options.TokenValidationParameters.RoleClaimType = JwtClaimTypes.Role;
                });

                //.AddWsFederation(options =>
                //{
                //    // Azure提供的一个链接
                //    options.MetadataAddress = "https://login.azurelink.com";
                //    // 登陆地址
                //    options.Wtrealm = "https://登录地址_自己的或者是微软的";
                //});

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();
        }
    }
}
