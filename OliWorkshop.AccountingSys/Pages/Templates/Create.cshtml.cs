﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OliWorkshop.AccountingSys.Data;

namespace OliWorkshop.AccountingSys.Pages.Templates
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly OliWorkshop.AccountingSys.Data.ApplicationDbContext _context;

        public CreateModel(OliWorkshop.AccountingSys.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public ConceptsTemplates ConceptsTemplates { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ConceptsTemplates.Add(ConceptsTemplates);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
