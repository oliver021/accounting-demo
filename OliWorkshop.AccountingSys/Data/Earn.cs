﻿using Microsoft.AspNetCore.Identity;
using OliWorkshop.AccountingSys.Data.Models;
using System;

namespace OliWorkshop.AccountingSys.Data
{
    public class Earn : IAssetAnotation
    {
        public uint Id { get; set; }

        public string Concept { get; set; }

        public decimal Amount { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        public uint EarnCategoryId { get; set; }

        public EarnCategory EarnCategory { get; set; }

        public DateTime AtCreated { get; set; }
    }
}
