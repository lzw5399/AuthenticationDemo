using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using JwtBearer.IdentityProvider.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JwtBearer.IdentityProvider.Controllers
{
    [Route("api/token")]
    public class TokenProviderController : ControllerBase
    {
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

            // jwt token处理器
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(secret);

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
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            var tokenString = tokenHandler.WriteToken(securityToken);

            return Ok(tokenString);
        }
    }
}