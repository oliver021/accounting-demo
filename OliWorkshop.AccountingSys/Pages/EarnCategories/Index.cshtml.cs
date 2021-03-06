﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OliWorkshop.AccountingSys.Data;
using System.Security.Claims;

namespace OliWorkshop.AccountingSys.Pages.EarnCategories
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<EarnCategory> EarnCategory { get;set; }

        public async Task OnGetAsync()
        {
            EarnCategory = await _context.EarnCategory
                .Where(x => x.UserId == HttpContext.User.GetUserId())
                .ToListAsync();
        }
    }
}
