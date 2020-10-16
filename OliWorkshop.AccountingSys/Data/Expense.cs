using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OliWorkshop.AccountingSys.Data
{
    public class Expense
    {
        public uint Id { get; set; }

        public string Concept { get; set; }

        public int Amount { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        public uint ExpenseCategoryId { get; set; } 

        public ExpenseCategory ExpenseCategory { get; set; }

        public DateTime AtCreated { get; set; }
    }
}
