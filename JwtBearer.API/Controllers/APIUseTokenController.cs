using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
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
            var name = User.Identity.Name;

            return Ok(name);
        }
    }
}
