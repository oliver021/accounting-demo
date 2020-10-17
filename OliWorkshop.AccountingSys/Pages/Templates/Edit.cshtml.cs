using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OliWorkshop.AccountingSys.Data;

namespace OliWorkshop.AccountingSys.Pages.Templates
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly OliWorkshop.AccountingSys.Data.ApplicationDbContext _context;

        public EditModel(OliWorkshop.AccountingSys.Data.ApplicationDbContext context)
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
           ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ConceptsTemplates).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConceptsTemplatesExists(ConceptsTemplates.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ConceptsTemplatesExists(uint id)
        {
            return _context.ConceptsTemplates.Any(e => e.Id == id);
        }
    }
}
