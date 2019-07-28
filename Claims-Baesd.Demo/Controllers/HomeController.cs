using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Claims_Baesd.Demo.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using IdentityModel;

namespace Claims_Baesd.Demo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string user, string pwd)
        {
            if (user != "admin" || pwd != "admin")
            {
                return RedirectToAction(nameof(Login));
            }

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

            var properties = new AuthenticationProperties();
            properties.IsPersistent = true;
            properties.ExpiresUtc = DateTime.UtcNow.AddMinutes(20);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, properties).Wait();

            return RedirectToAction(nameof(Manager));
        }

        [Authorize(Roles = "C1")]
        public IActionResult Manager()
        {
            return Content($"Hello,{User?.Identity?.Name}");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();

            return RedirectToAction(nameof(Login));
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