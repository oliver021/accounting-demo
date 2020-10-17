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
    public class ExpensesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpense(int page = 0, int length = 45)
        {
            if (page == 0)
            {
                // return all elements
                return await _context.Expense.ToListAsync();
            }
            else if (page > 0 && length > 0)
            {
                // return paginate list
                return await _context.Expense.Skip(length * (page - 1)).Take(length).ToListAsync();
            }
            else
            {
                // the request is invalid
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpense(uint id)
        {
            var expense = await _context.Expense.FindAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            return expense;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpense(uint id, Expense expense)
        {
            if (id != expense.Id)
            {
                return BadRequest();
            }

            _context.Entry(expense).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(id))
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
        public async Task<ActionResult<Expense>> PostExpense(Expense expense)
        {
            _context.Expense.Add(expense);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExpense", new { id = expense.Id }, expense);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Expense>> DeleteExpense(uint id)
        {
            var expense = await _context.Expense.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            _context.Expense.Remove(expense);
            await _context.SaveChangesAsync();

            return expense;
        }

        private bool ExpenseExists(uint id)
        {
            return _context.Expense.Any(e => e.Id == id);
        }
    }
}
