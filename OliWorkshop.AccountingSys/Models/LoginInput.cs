﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OliWorkshop.AccountingSys.Models
{
    public class LoginInput
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(128)]
        public string Password { get; set; }
        public bool RemberMe { get; set; } = false;
    }
}
