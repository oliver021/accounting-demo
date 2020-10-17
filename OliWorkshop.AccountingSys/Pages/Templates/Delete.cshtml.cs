using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OliWorkshop.AccountingSys.Data;

namespace OliWorkshop.AccountingSys.Pages.Templates
{
    public class DeleteModel : PageModel
    {
        private readonly OliWorkshop.AccountingSys.Data.ApplicationDbContext _context;

        public DeleteModel(OliWorkshop.AccountingSys.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ConceptsTemplates ConceptsTemplates { get; set; }

        public async Task<IActionResult> OnGetAsync(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ConceptsTemplates = await _context.ConceptsTemplates
                .Include(c => c.User).FirstOrDefaultAsync(m => m.Id == id);

            if (ConceptsTemplates == null)
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

            ConceptsTemplates = await _context.ConceptsTemplates.FindAsync(id);

            if (ConceptsTemplates != null)
            {
                _context.ConceptsTemplates.Remove(ConceptsTemplates);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
