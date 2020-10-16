using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using OliWorkshop.AccountingSys.Data;

namespace OliWorkshop.AccountingSys.Pages.Expenses
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(OliWorkshop.AccountingSys.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Expense> Expense { get;set; }

        public async Task OnGetAsync()
        {
            Expense = await ResolveQuery().ToListAsync();
        }

        /// <summary>
        /// Crate a smart filter to manage query record from query params
        /// </summary>
        /// <returns></returns>
        private IQueryable<Expense> ResolveQuery()
        {
            var category = HttpContext.Request.Query["category"];
            var before = HttpContext.Request.Query["before"];
            var after = HttpContext.Request.Query["after"];
            var amount_min = HttpContext.Request.Query["amount_min"];
            var amount_max = HttpContext.Request.Query["amount_max"];
            IQueryable<Expense> result = _context.Expense;

            if (category != StringValues.Empty && uint.TryParse(category.ToString(), out uint value))
            {
                result = result.Where(x => x.ExpenseCategoryId == value);
            }

            if (before != StringValues.Empty && DateTime.TryParse(before, out DateTime instanceBefore))
            {
                result = result.Where(x => x.AtCreated < instanceBefore);
            }

            if (before != StringValues.Empty && DateTime.TryParse(after, out DateTime instanceAfter))
            {
                result = result.Where(x => x.AtCreated > instanceAfter);
            }

            if (amount_min != StringValues.Empty && int.TryParse(amount_min.ToString(), out int instanceMin))
            {
                result = result.Where(x => x.Amount >= instanceMin);
            }

            if (amount_max != StringValues.Empty && int.TryParse(amount_min.ToString(), out int instanceMax))
            {
                result = result.Where(x => x.Amount <= instanceMax);
            }

            return result.Include(e => e.ExpenseCategory);
        }
    }
}
