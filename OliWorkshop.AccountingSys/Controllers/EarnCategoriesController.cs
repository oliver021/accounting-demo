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
        private readonly ApplicationDbContext Context;

        public EarnCategoriesController(ApplicationDbContext context)
        {
            Context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EarnCategory>>> GetEarnCategory(int page = 0, int length = 45)
        {
            if (page == 0)
            {
                // return all elements
                return await Context.EarnCategory.ToListAsync();
            }else if (page > 0 && length > 0)
            {
                // return paginate list
                return await Context.EarnCategory.Skip(length*(page-1)).Take(length).ToListAsync();
            }
            else
            {
                // the request is invalid
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EarnCategory>> GetEarnCategory(uint id)
        {
            // validate id value
            if (id == default)
            {
                return BadRequest();
            }

            var earnCategory = await Context.EarnCategory.FindAsync(id);

            if (earnCategory == null)
            {
                return NotFound();
            }

            return earnCategory;
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEarnCategory(uint id, EarnCategory earnCategory)
        {
            // validate id value
            if (id == default)
            {
                return BadRequest();
            }

            if (id != earnCategory.Id)
            {
                return BadRequest();
            }

            Context.Entry(earnCategory).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await Context.EarnCategory.AnyAsync(e => e.Id == id))
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

        [HttpPost]
        public async Task<ActionResult> PostEarnCategory(EarnCategory earnCategory)
        {
            // the name category should be unique
            if (await Context.EarnCategory.AnyAsync(x => x.Name == earnCategory.Name))
            {
                return BadRequest("The name category should be unique.");
            }

            Context.EarnCategory.Add(earnCategory);
            await Context.SaveChangesAsync();

            return Ok( new { id = earnCategory.Id });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEarnCategory(uint id)
        {
            // validate id value
            if (id == default)
            {
                return BadRequest();
            }

            var earnCategory = await Context.EarnCategory.FindAsync(id);
            if (earnCategory == null)
            {
                return NotFound();
            }

            Context.EarnCategory.Remove(earnCategory);
            await Context.SaveChangesAsync();

            return NoContent();
        }
    }
}
