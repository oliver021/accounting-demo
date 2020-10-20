using Microsoft.AspNetCore.Localization;
using OliWorkshop.AccountingSys.Components;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static  class StartupConfigurationExtensions
    {
        public static void UseMultiCulture(this IApplicationBuilder app)
        {
            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("es"),
                new CultureInfo("en")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(supportedCultures.First()),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,

                // add from std header browser
                RequestCultureProviders = new List<IRequestCultureProvider> { 
                    new UserPreferenceCultureProvider(),
                    new AcceptLanguageHeaderRequestCultureProvider()
                }
            });
        }
    }
}
