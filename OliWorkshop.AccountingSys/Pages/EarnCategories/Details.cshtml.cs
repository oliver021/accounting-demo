using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OliWorkshop.AccountingSys.Data;
using OliWorkshop.AccountingSys.Data.Models;
using OliWorkshop.AccountingSys.Helpers;
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

            // 404
            if (EarnCategory == null)
            {
                return NotFound();
            }

            // resolve basic metrics like total and many types of avarage base on date
            await DataHelpers.ResolveResume(query: _context.Earn.Where(x => x.EarnCategoryId == EarnCategory.Id),
                                            resume: Resume);

            return Page();
        }

        
    }
}
