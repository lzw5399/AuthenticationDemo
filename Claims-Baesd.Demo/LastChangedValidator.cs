using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Claims_Baesd.Demo
{
    public static class LastChangedValidator
    {
        public static async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            ClaimsPrincipal userPrincipal = context.Principal;
            string lastChanged = (from c in userPrincipal.Claims where c.Type == "LastUpdated" select c.Value).FirstOrDefault();
            if (string.IsNullOrEmpty(lastChanged) || !userRepository.ValidateLastChanged(userPrincipal, lastChanged))
            {
                // 1. 验证失败 等同于 Principal = principal;
                context.RejectPrincipal();

                // 2. 验证通过，并会重新生成Cookie。
                context.ShouldRenew = true;

                await context.HttpContext.SignOutAsync();
            }
        }
    }

    public interface IUserRepository
    {
        bool ValidateLastChanged(ClaimsPrincipal principal, string lastChanged);
    }
}