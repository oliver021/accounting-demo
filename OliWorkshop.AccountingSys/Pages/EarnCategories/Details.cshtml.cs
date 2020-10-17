using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OliWorkshop.AccountingSys.Data;
using OliWorkshop.AccountingSys.Models;

namespace OliWorkshop.AccountingSys.Pages.EarnCategories
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public EarnCategory EarnCategory { get; set; }

        public Resume Resume { get; set; } = new Resume();

        public async Task<IActionResult> OnGetAsync(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EarnCategory = await _context.EarnCategory
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            var query = _context.Earn.Where(x => x.EarnCategoryId == EarnCategory.Id);


            Resume.Total = await query.SumAsync(x => x.Amount);
            Resume.AvarageTotal = await query.AverageAsync(x => (decimal) x.Amount);

            Resume.AvaragePerYear = await query
                .GroupBy(x => x.AtCreated.Year)
                .Select(x => x.Sum(j => (decimal) j.Amount))
                .AverageAsync();

            Resume.AvaragePerMonth = await query
               .GroupBy(x => x.AtCreated.Month)
               .Select(x => x.Sum(j => (decimal) j.Amount))
               .AverageAsync();

            Resume.AvaragePerDay = await query
               .GroupBy(x => x.AtCreated.Day)
               .Select(x => x.Sum(j => (decimal)j.Amount))
               .AverageAsync();

            if (EarnCategory == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
