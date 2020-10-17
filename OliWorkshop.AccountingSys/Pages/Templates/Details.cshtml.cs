using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OliWorkshop.AccountingSys.Data;

namespace OliWorkshop.AccountingSys.Pages.Templates
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly OliWorkshop.AccountingSys.Data.ApplicationDbContext _context;

        public DetailsModel(OliWorkshop.AccountingSys.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
