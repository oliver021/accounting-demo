using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OliWorkshop.AccountingSys.Data;
using OliWorkshop.AccountingSys.Models;

namespace OliWorkshop.AccountingSys.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        public RegisterModel(SignInManager<User> signInManager, UserManager<User> user)
        {
            SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            UserManager = user ?? throw new ArgumentNullException(nameof(user));
        }

        /// <summary>
        /// Binding the user login Input
        /// </summary>
        [BindProperty]
        public RegisterInput Registration { get; set; }

        /// <summary>
        /// artifact to login an user
        /// </summary>
        public SignInManager<User> SignInManager { get; }

        /// <summary>
        /// is necesary to put custom claims for this system
        /// </summary>
        public UserManager<User> UserManager { get; }

        /// <summary>
        /// The eerros list to register
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// Process Login request
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // get the culture provide by request
                var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
                var culture = requestCulture.RequestCulture.UICulture;

                // check the password confirmation
                if (!Registration.Password.Equals(Registration.ConfirmPassword))
                {
                    Errors = new List<string>() { "The password confirmation is not match" };
                    return Page();
                }

                // create a new user for registration
                var newUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = Registration.Email,
                    Email = Registration.Email,
                    Fullname = Registration.Fullname,
                    Locale = culture.Name
                };

                // request to db for a new user
                var resultRegistration = await UserManager.CreateAsync(newUser, Registration.Password);

                // check if the registration has been Succeeded
                if (!resultRegistration.Succeeded)
                {
                    Errors = resultRegistration.Errors.Select(x => x.Description).ToList();
                    return Page();
                }

                // add claims with locale data that provide the user culture by preference
                await UserManager.AddClaimAsync(newUser, new Claim(ClaimTypes.Locality, newUser.Locale));

                // check if the user requested start session
                if (Registration.StartSessionNow)
                {
                    var result = await SignInManager.PasswordSignInAsync(Registration.Email, Registration.Password, Registration.RemberMe, false);

                    if (!result.Succeeded)
                    {
                        return Page();
                    }

                    return Redirect("/Index");
                }
                else
                {
                    return Redirect("/Identity/Account/Login");
                }
            }
            else
            {
                return Page();
            }
        }
    }
}
