using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using JwtBearer.IdentityProvider.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace JwtBearer.IdentityProvider.Controllers
{
    [Route("api/token")]
    public class TokenProviderController : ControllerBase
    {
        private readonly IHostingEnvironment _env;

        public TokenProviderController(IHostingEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        public IActionResult Login([FromBody]LoginRequest request)
        {
            if (request.UserName != "admin" || request.Password != "admin")
            {
                return BadRequest("用户名或密码错误");
            }

            // mock用户登录成功的信息
            var user = new User
            {
                Id = 1,
                Name = "zhangsan",
                Email = "zhangsan@gmail.com",
                Password = "123456",
                BirthDay = DateTime.Parse("2018.1.1"),
                PhoneNumber = "0123456789"
            };

            // mock jwt的密钥secret
            var secret = "Thisisthesecretkey!@#$%^&*()_+";
            var key = Encoding.ASCII.GetBytes(secret);

            // jwt token处理器
            var tokenHandler = new JwtSecurityTokenHandler();

            // 读取私钥文件，并序列化, 并通过keyParameters构建一个rsaSecurityKey
            //var privateKey = System.IO.File.ReadAllText(Path.Combine(_env.ContentRootPath, "key-private.json"));
            //var keyParameters = JsonConvert.DeserializeObject<RSAParameters>(privateKey);
            //var rsaSecurityKey = new RsaSecurityKey(keyParameters);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtClaimTypes.Audience, "api"),// 接收jwt的一方
                    new Claim(JwtClaimTypes.Issuer, "http://localhost:59541"),// 签发者
                    new Claim(JwtClaimTypes.Id, user.Id.ToString()),
                    new Claim(JwtClaimTypes.Name, user.Name),
                    new Claim(JwtClaimTypes.Email, user.Email),
                    new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
                    new Claim(JwtClaimTypes.Role, "manager")
                }),
                Expires = DateTime.UtcNow.AddMinutes(20),
                // SigningCredentials = new SigningCredentials(new RsaSecurityKey(keyParameters), SecurityAlgorithms.RsaSha256Signature)
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            var tokenString = tokenHandler.WriteToken(securityToken);

            return Ok(tokenString);
        }
    }
}