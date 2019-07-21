using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityDemo.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;

namespace IdentityDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // 声明
            var nameClaim = new Claim(ClaimTypes.Name, "bs");
            var emailClaim = new Claim(ClaimTypes.Email, "zhiwen@kooboo.cn");
            var roleClaim = new Claim(ClaimTypes.Role, "C1");

            // 证件1
            var claimsIdentity = new ClaimsIdentity("身份证");
            claimsIdentity.AddClaim(nameClaim);
            claimsIdentity.AddClaim(emailClaim);

            // 证件2
            var claimsIdentity2 = new ClaimsIdentity("驾驶证");
            claimsIdentity2.AddClaim(roleClaim);

            // 当事人(User)
            var userPrincipal = new ClaimsPrincipal(claimsIdentity);
            userPrincipal.AddIdentity(claimsIdentity);
            userPrincipal.AddIdentity(claimsIdentity2);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal).Wait();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
