using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OliWorkshop.AccountingSys.Data;

namespace OliWorkshop.AccountingSys.Pages.ExpenseCaregories
{
    public class DeleteModel : PageModel
    {
        private readonly OliWorkshop.AccountingSys.Data.ApplicationDbContext _context;

        public DeleteModel(OliWorkshop.AccountingSys.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ExpenseCategory ExpenseCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ExpenseCategory = await _context.ExpenseCategory
                .Include(e => e.User).FirstOrDefaultAsync(m => m.Id == id);

            if (ExpenseCategory == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ExpenseCategory = await _context.ExpenseCategory.FindAsync(id);

            if (ExpenseCategory != null)
            {
                _context.ExpenseCategory.Remove(ExpenseCategory);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
