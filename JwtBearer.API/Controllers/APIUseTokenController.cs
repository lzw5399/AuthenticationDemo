using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtBearer.API.Controllers
{
    [Route("api/usetoken")]
    public class APIUseTokenController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult GetAuthorizedName()
        {
            var name = User.Identity.Name ?? "认证失败";

            return Ok(name);
        }
    }
}
