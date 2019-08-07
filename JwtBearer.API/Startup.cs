using System;
using System.Collections.Generic;
using System.IO;
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
            var rsa = GetRSAParameters();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // 【对称加密】mock jwt的密钥secret
                    //var secret = "Thisisthesecretkey!@#$%^&*()_+";
                    //var key = Encoding.ASCII.GetBytes(secret);
                    //options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(key);

                    // 【非对称加密】
                    options.TokenValidationParameters.IssuerSigningKey = new RsaSecurityKey(rsa);

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

        private RSAParameters GetRSAParameters()
        {
            var d = GetValueForRsa(Configuration.GetValue<string>("rsa:D"));
            var dP = GetValueForRsa(Configuration.GetValue<string>("rsa:DP"));
            var dQ = GetValueForRsa(Configuration.GetValue<string>("rsa:DQ"));
            var exponent = GetValueForRsa(Configuration.GetValue<string>("rsa:Exponent"));
            var inverseQ = GetValueForRsa(Configuration.GetValue<string>("rsa:InverseQ"));
            var modulus = GetValueForRsa(Configuration.GetValue<string>("rsa:Modulus"));
            var p = GetValueForRsa(Configuration.GetValue<string>("rsa:P"));
            var q = GetValueForRsa(Configuration.GetValue<string>("rsa:Q"));

            var rsa = new { D = d, DP = dP, DQ = dQ, Exponent = exponent, InverseQ = inverseQ, Modulus = modulus, P = p, Q = q };

            var rsaStr = JsonConvert.SerializeObject(rsa);
            var rsaObj = JsonConvert.DeserializeObject<RSAParameters>(rsaStr);

            return rsaObj;
        }

        public string GetValueForRsa(string str)
        {
            return !string.IsNullOrEmpty(str) ? str : null;
        }
    }
}
