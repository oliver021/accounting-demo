using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using OliWorkshop.AccountingSys.Data;
using OliWorkshop.AccountingSys.Models;

namespace OliWorkshop.AccountingSys.Pages.Earns
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FilterOptions FilterOptions { get; set; }

        public IList<Earn> Earn { get;set; }

        public async Task OnGetAsync()
        {
            ViewData["EarnCategoryId"] = new SelectList(
            _context.EarnCategory
            .Where(x => x.UserId == HttpContext.User.GetUserId()), "Id", "Name");
            Earn = await ResolveQuery().ToListAsync();
        }

        /// <summary>
        /// Crate a smart filter to manage query record from query params
        /// </summary>
        /// <returns></returns>
        private IQueryable<Earn> ResolveQuery()
        {
            var category = HttpContext.Request.Query["category"];
            var term = HttpContext.Request.Query["term"];
            var before = HttpContext.Request.Query["before"];
            var after = HttpContext.Request.Query["after"];
            var amount_min = HttpContext.Request.Query["amount_min"];
            var amount_max = HttpContext.Request.Query["amount_max"];
            IQueryable<Earn> result = _context.Earn;

            if (term != StringValues.Empty)
            {
                result = result.Where(x => x.Concept.Contains(term.ToString()));
            }

            if (category != StringValues.Empty && uint.TryParse(category.ToString(), out uint value))
            {
                result = result.Where(x => x.EarnCategoryId == value);
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

            if (amount_max != StringValues.Empty && int.TryParse(amount_max.ToString(), out int instanceMax))
            {
                result = result.Where(x => x.Amount <= instanceMax);
            }

            return result
                .Where(x => x.UserId == HttpContext.User.GetUserId())
                .OrderByDescending(x => x.AtCreated)
                .Include(e => e.EarnCategory);
        }
    }
}
