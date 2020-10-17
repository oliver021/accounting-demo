using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OliWorkshop.AccountingSys.Data;
using OliWorkshop.AccountingSys.Helpers;
using OliWorkshop.AccountingSys.Models;

namespace OliWorkshop.AccountingSys.Pages.ExpenseCaregories
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Resume Resume { get; set; } = new Resume();

        public ExpenseCategory ExpenseCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ExpenseCategory = await _context.ExpenseCategory
                .Include(e => e.User).FirstOrDefaultAsync(m => m.Id == id);

            // 404
            if (ExpenseCategory == null)
            {
                return NotFound();
            }

            // resolve basic metrics like total and many types of avarage base on date
            await DataHelpers.ResolveResume(query: _context.Expense.Where(x => x.ExpenseCategoryId == ExpenseCategory.Id),
                                            resume: Resume);

            return Page();
        }
    }
}
