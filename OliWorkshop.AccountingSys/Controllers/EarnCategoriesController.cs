using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OliWorkshop.AccountingSys.Data;

namespace OliWorkshop.AccountingSys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EarnCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EarnCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EarnCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EarnCategory>>> GetEarnCategory()
        {
            return await _context.EarnCategory.ToListAsync();
        }

        // GET: api/EarnCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EarnCategory>> GetEarnCategory(uint id)
        {
            var earnCategory = await _context.EarnCategory.FindAsync(id);

            if (earnCategory == null)
            {
                return NotFound();
            }

            return earnCategory;
        }

        // PUT: api/EarnCategories/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEarnCategory(uint id, EarnCategory earnCategory)
        {
            if (id != earnCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(earnCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EarnCategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EarnCategories
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<EarnCategory>> PostEarnCategory(EarnCategory earnCategory)
        {
            _context.EarnCategory.Add(earnCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEarnCategory", new { id = earnCategory.Id }, earnCategory);
        }

        // DELETE: api/EarnCategories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EarnCategory>> DeleteEarnCategory(uint id)
        {
            var earnCategory = await _context.EarnCategory.FindAsync(id);
            if (earnCategory == null)
            {
                return NotFound();
            }

            _context.EarnCategory.Remove(earnCategory);
            await _context.SaveChangesAsync();

            return earnCategory;
        }

        private bool EarnCategoryExists(uint id)
        {
            return _context.EarnCategory.Any(e => e.Id == id);
        }
    }
}
