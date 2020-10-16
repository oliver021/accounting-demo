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
        private readonly ApplicationDbContext _context;

        public EarnsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Earns
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Earn>>> GetEarn()
        {
            return await _context.Earn.ToListAsync();
        }

        // GET: api/Earns/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Earn>> GetEarn(uint id)
        {
            var earn = await _context.Earn.FindAsync(id);

            if (earn == null)
            {
                return NotFound();
            }

            return earn;
        }

        // PUT: api/Earns/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEarn(uint id, Earn earn)
        {
            if (id != earn.Id)
            {
                return BadRequest();
            }

            _context.Entry(earn).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EarnExists(id))
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

        // POST: api/Earns
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Earn>> PostEarn(Earn earn)
        {
            _context.Earn.Add(earn);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEarn", new { id = earn.Id }, earn);
        }

        // DELETE: api/Earns/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Earn>> DeleteEarn(uint id)
        {
            var earn = await _context.Earn.FindAsync(id);
            if (earn == null)
            {
                return NotFound();
            }

            _context.Earn.Remove(earn);
            await _context.SaveChangesAsync();

            return earn;
        }

        private bool EarnExists(uint id)
        {
            return _context.Earn.Any(e => e.Id == id);
        }
    }
}
