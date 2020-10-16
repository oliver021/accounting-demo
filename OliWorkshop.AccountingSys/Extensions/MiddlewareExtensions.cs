using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class MiddlewareExtensions
    {
        public static void EnsureUserIdPresent(this IApplicationBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Use(async (context, next) => {
                var signIn = context.RequestServices.GetService<SignInManager<IdentityUser>>();
                if (signIn.IsSignedIn(context.User))
                {
                    Console.WriteLine(context.User.Identities.Count());
                }
                await next.Invoke();
            });
        }
    }
}
