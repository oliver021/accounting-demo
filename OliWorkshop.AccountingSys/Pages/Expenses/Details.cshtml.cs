using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OliWorkshop.AccountingSys.Data;

namespace OliWorkshop.AccountingSys.Pages.Expenses
{
    public class DetailsModel : PageModel
    {
        private readonly OliWorkshop.AccountingSys.Data.ApplicationDbContext _context;

        public DetailsModel(OliWorkshop.AccountingSys.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Expense Expense { get; set; }

        public async Task<IActionResult> OnGetAsync(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Expense = await _context.Expense
                .Include(e => e.ExpenseCategory)
                .Include(e => e.User).FirstOrDefaultAsync(m => m.Id == id);

            if (Expense == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
