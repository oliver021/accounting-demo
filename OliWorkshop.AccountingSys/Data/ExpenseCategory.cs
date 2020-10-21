using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OliWorkshop.AccountingSys.Data
{
    public class ExpenseCategory
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public DateTime AtCreated { get; set; }

        public List<CategoryExpenseGroup> Groups { get; set; }
    }
}
