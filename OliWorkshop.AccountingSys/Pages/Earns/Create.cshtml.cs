﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OliWorkshop.AccountingSys.Data;

namespace OliWorkshop.AccountingSys.Pages.Earns
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["EarnCategoryId"] = new SelectList(
            _context.EarnCategory
            .Where(x => x.UserId == HttpContext.User.GetUserId()), "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Earn Earn { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Earn.Add(Earn);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
