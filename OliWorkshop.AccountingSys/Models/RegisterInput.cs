using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OliWorkshop.AccountingSys.Models
{
    public class RegisterInput : LoginInput
    {
        public string Fullname { get; set; }

        public string ConfirmPassword { get; set; }

        public bool StartSessionNow { get; set; }
    }
}
