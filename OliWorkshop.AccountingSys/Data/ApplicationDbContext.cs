using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OliWorkshop.AccountingSys.Data;

namespace OliWorkshop.AccountingSys.Data
{
    /// <summary>
    /// The entities data contex is important service to work with data
    /// </summary>
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
        public DbSet<CountableGroup> CountableGroup { get; set; }
        public DbSet<Log> Logs { get; set; }

        /// <summary>
        /// Building a model with complex relations that allow create 
        /// custom griuo by user
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // use parent specification
            base.OnModelCreating(builder);

            /// complex set of relations and some features like index
            builder.Entity<CountableGroup>(entity => {
                entity.HasMany(x => x.CategoryEarnGroups);
                entity.HasMany(x => x.CategoryExpenseGroups);
                entity.HasIndex(x => x.Name)
                .IsUnique();
            });

            builder.Entity<CategoryEarnGroup>(entity => {
                entity.HasOne(x => x.CountableGroup);
                entity.HasOne(x => x.EarnCategory);
            });

            builder.Entity<CategoryExpenseGroup>(entity => {
                entity.HasOne(x => x.CountableGroup);
                entity.HasOne(x => x.ExpenseCategory);
            });

            builder.Entity<ExpenseCategory>(entity => {
                entity.HasMany(x => x.Groups);
                entity.HasIndex(x => x.Name)
                .IsUnique();
            });

            builder.Entity<EarnCategory>(entity => {
                entity.HasMany(x => x.Groups);
                entity.HasIndex(x => x.Name)
                .IsUnique();
            });
        }
    }
}
