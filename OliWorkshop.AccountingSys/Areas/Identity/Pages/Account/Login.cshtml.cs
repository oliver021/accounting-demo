using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OliWorkshop.AccountingSys.Data;
using OliWorkshop.AccountingSys.Models;
using LoginResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace OliWorkshop.AccountingSys.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        public LoginModel(SignInManager<User> signInManager, UserManager<User> user)
        {
            SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            UserManager = user ?? throw new ArgumentNullException(nameof(user));
        }

        /// <summary>
        /// Binding the user login Input
        /// </summary>
        [BindProperty]
        public LoginInput Login { get; set; }

        /// <summary>
        /// artifact to login an user
        /// </summary>
        public SignInManager<User> SignInManager { get; }
        
        /// <summary>
        /// is necesary to put custom claims for this system
        /// </summary>
        public UserManager<User> UserManager { get; }

        /// <summary>
        /// List of message to inforn about errors
        /// </summary>
        public List<string> LoginsErrors { get; } = new List<string>();

        /// <summary>
        /// Process Login request
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await SignInManager.PasswordSignInAsync(Login.Email, Login.Password, Login.RemberMe, false);
                
                if (!result.Succeeded)
                {
                    AnalizeProblems(result);
                    return Page();
                }

                var currentUserInstance = await UserManager.FindByNameAsync(Login.Email);
                await UserManager.AddClaimAsync(currentUserInstance, new Claim(ClaimTypes.Locality, currentUserInstance.Locale));
                return Redirect("/Index");
            }
            else
            {
                return Page();
            }
        }

        /// <summary>
        /// Fetch info for login problems
        /// </summary>
        /// <param name="result"></param>
        private void AnalizeProblems(LoginResult result)
        {
            if (result.IsLockedOut)
            {
                LoginsErrors.Add("You can authenticate for now");
            }

            if (result.IsNotAllowed)
            {
                LoginsErrors.Add("The authentication is not allowd");
            }

            if (result.IsNotAllowed && result.IsLockedOut && result.RequiresTwoFactor)
            {
                LoginsErrors.Add("The crendentials is not correct, review email and password");
            }
        }
    }
}
