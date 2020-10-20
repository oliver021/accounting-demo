using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OliWorkshop.AccountingSys.Data
{
    public class User : IdentityUser<string>
    {
        /// <summary>
        /// The locale specification
        /// </summary>
        public string Locale { get; set; }
    }
}
