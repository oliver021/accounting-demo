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
    public class EarnsController : ControllerBase
    {
        private readonly ApplicationDbContext Context;

        public EarnsController(ApplicationDbContext context)
        {
            Context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Earn>>> GetEarn(int page = 0, int length = 45)
        {
            if (page == 0)
            {
                // return all elements
                return await Context.Earn.ToListAsync();
            }
            else if (page > 0 && length > 0)
            {
                // return paginate list
                return await Context.Earn.Skip(length * (page - 1)).Take(length).ToListAsync();
            }
            else
            {
                // the request is invalid
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Earn>> GetEarn(uint id)
        {
            // validate id value
            if (id == default)
            {
                return BadRequest();
            }

            var earn = await Context.Earn.FindAsync(id);

            // check if exists
            if (earn == null)
            {
                return NotFound();
            }

            return earn;
        }

      
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEarn(uint id, Earn earn)
        {
            // validate id value
            if (id == default)
            {
                return BadRequest();
            }
            
            // check the result
            if (id != earn.Id)
            {
                return BadRequest();
            }

            Context.Entry(earn).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // check the problems
                if (! await Context.Earn.AnyAsync(e => e.Id == id))
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
        public async Task<ActionResult<Earn>> PostEarn(Earn earn)
        {
            Context.Earn.Add(earn);
            await Context.SaveChangesAsync();

            return Ok( new { id = earn.Id });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<NoContentResult>> DeleteEarn(uint id)
        {
            // validate id value
            if (id == default)
            {
                return BadRequest();
            }


            var earn = await Context.Earn.FindAsync(id);
            if (earn == null)
            {
                return NotFound();
            }

            Context.Earn.Remove(earn);
            await Context.SaveChangesAsync();

            return NoContent();
        }
    }
}
