using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OliWorkshop.AccountingSys.Data;

namespace OliWorkshop.AccountingSys.Pages.ExpenseCaregories
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ExpenseCategory> ExpenseCategory { get;set; }

        public async Task OnGetAsync()
        {
            ExpenseCategory = await _context.ExpenseCategory
                .Where(x => x.UserId == HttpContext.User.GetUserId())
                .Include(e => e.User).ToListAsync();
        }
    }
}
