using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OliWorkshop.AccountingSys.Data;

namespace OliWorkshop.AccountingSys.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<EarnCategory> EarnCategory { get; set; }
        public DbSet<Earn> Earn { get; set; }
        public DbSet<OliWorkshop.AccountingSys.Data.ExpenseCategory> ExpenseCategory { get; set; }
        public DbSet<OliWorkshop.AccountingSys.Data.Expense> Expense { get; set; }
    }
}
