using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace System.Security.Claims
{
    public static class CommonsExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            try
            {
                return user.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).First().Value;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
