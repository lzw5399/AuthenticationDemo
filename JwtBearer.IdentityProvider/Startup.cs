using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace JwtBearer.IdentityProvider
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
            var rsa = GetRSAParameters();
            services.AddSingleton(typeof(RSAParameters), rsa);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 给API项目添加默认路由
            app.UseMvcWithDefaultRoute();
        }

        private RSAParameters GetRSAParameters()
        {
            var d = GetValueForRsa(Configuration.GetValue<string>("rsa-private:D"));
            var dP = GetValueForRsa(Configuration.GetValue<string>("rsa-private:DP"));
            var dQ = GetValueForRsa(Configuration.GetValue<string>("rsa-private:DQ"));
            var exponent = GetValueForRsa(Configuration.GetValue<string>("rsa-private:Exponent"));
            var inverseQ = GetValueForRsa(Configuration.GetValue<string>("rsa-private:InverseQ"));
            var modulus = GetValueForRsa(Configuration.GetValue<string>("rsa-private:Modulus"));
            var p = GetValueForRsa(Configuration.GetValue<string>("rsa-private:P"));
            var q = GetValueForRsa(Configuration.GetValue<string>("rsa-private:Q"));

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
