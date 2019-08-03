using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JWT.Demo.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using IdentityModel;
using JWT.Demo.Jwt;

namespace JWT.Demo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var token = JwtHelper.GenerateJwt();

            var isValid = JwtHelper.ValidateJwt(token);

            return Content(token);
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
