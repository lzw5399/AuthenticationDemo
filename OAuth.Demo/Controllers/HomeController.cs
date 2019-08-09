using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OAuth.Demo.Models;

namespace OAuth.Demo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            var x = HttpContext.User;
            var access_token = HttpContext.GetTokenAsync("access_token").Result;
            var id_token = HttpContext.GetTokenAsync("id_token").Result;
            return View();
        }

        [Authorize]
        [ActionName("signin-microsoft")]
        public IActionResult Hello()
        {
            var access_token = HttpContext.GetTokenAsync("access_token").Result;
            var id_token = HttpContext.GetTokenAsync("id_token").Result;

            var result = new { name = HttpContext.User.Identity.Name, access = access_token, id = id_token };
            return Json(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
