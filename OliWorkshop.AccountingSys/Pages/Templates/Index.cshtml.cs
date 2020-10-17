using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OliWorkshop.AccountingSys.Data;

namespace OliWorkshop.AccountingSys.Pages.Templates
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ConceptsTemplates> ConceptsTemplates { get;set; }

        public async Task OnGetAsync()
        {
            ConceptsTemplates = await _context.ConceptsTemplates
                .Where(x => x.UserId == HttpContext.User.GetUserId())
                .ToListAsync();
        }
    }
}
