using Microsoft.AspNetCore.Identity;
using System;

namespace OliWorkshop.AccountingSys.Data
{
    public class Earn
    {
        public uint Id { get; set; }

        public string Concept { get; set; }

        public int Amount { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        public uint EarnCategoryId { get; set; }

        public EarnCategory EarnCategory { get; set; }

        public DateTime AtCreated { get; set; }
    }
}
