using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using OliWorkshop.AccountingSys.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace OliWorkshop.AccountingSys.Components
{
    public class UserPreferenceCultureProvider : IRequestCultureProvider
    {
        /// <summary>
        /// Resolve culture UI by user preference's
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var SignIn = httpContext.RequestServices.GetService<SignInManager<User>>();

            // if not signed then return null and the next culture provider should to resolver
            if (SignIn.IsSignedIn(httpContext.User))
            {
                // return a culture provider
                return null;
            }

            // find the locality claim
            var claim = httpContext.User.FindFirst(ClaimTypes.Locality);

            // declare the initial type for cultureResult
            ProviderCultureResult cultureResult;

            // if not set then pass null
            if (claim != null)
            {
                cultureResult = new ProviderCultureResult(Thread.CurrentThread.CurrentCulture.Name, claim.Value);
            }
            else
            {
                // fallback resolve
                var userManager = httpContext.RequestServices.GetService<AspNetUserManager<User>>();
                var currentUser = await userManager.GetUserAsync(httpContext.User);
                await userManager.AddClaimAsync(currentUser, new Claim(ClaimTypes.Locality, currentUser.Locale));
                cultureResult = new ProviderCultureResult(Thread.CurrentThread.CurrentCulture.Name, currentUser.Locale);
            }

            // return a culture provider
            return cultureResult;
        }
    }
}
