using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OliWorkshop.AccountingSys.Data;

namespace OliWorkshop.AccountingSys.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<EarnCategory> EarnCategory { get; set; }
        public DbSet<Earn> Earn { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategory { get; set; }
        public DbSet<Expense> Expense { get; set; }
        public DbSet<ConceptsTemplates> ConceptsTemplates { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}
