using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OliWorkshop.AccountingSys.Data;

namespace OliWorkshop.AccountingSys.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        public LogoutModel(SignInManager<User> signInManager)
        {
            SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        public SignInManager<User> SignInManager { get; }

        /// <summary>
        /// Close the current user session
        /// </summary>
        /// <returns></returns>
        public async Task<RedirectResult> OnPostAsync()
        {
            await SignInManager.SignOutAsync();
            return Redirect("/Index");
        }
    }
}
