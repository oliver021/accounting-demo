using Microsoft.AspNetCore.Identity;
using OliWorkshop.AccountingSys.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OliWorkshop.AccountingSys.Data
{
    public class Expense : IAssetAnotation
    {
        public uint Id { get; set; }

        public string Concept { get; set; }

        public decimal Amount { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        public uint ExpenseCategoryId { get; set; } 

        public ExpenseCategory ExpenseCategory { get; set; }

        public DateTime AtCreated { get; set; }

        [NotMapped]
        public string TextDate { get; set; }

        [NotMapped]
        public string TextDateAgo { get; set; }
    }
}
