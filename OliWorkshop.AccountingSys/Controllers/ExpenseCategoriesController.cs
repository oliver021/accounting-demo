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
    public class ExpenseCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExpenseCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ExpenseCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseCategory>>> GetExpenseCategory(int page = 0, int length = 45)
        {
            if (page == 0)
            {
                // return all elements
                return await _context.ExpenseCategory.ToListAsync();
            }
            else if (page > 0 && length > 0)
            {
                // return paginate list
                return await _context.ExpenseCategory.Skip(length * (page - 1)).Take(length).ToListAsync();
            }
            else
            {
                // the request is invalid
                return BadRequest();
            }
        }

        // GET: api/ExpenseCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseCategory>> GetExpenseCategory(uint id)
        {
            var expenseCategory = await _context.ExpenseCategory.FindAsync(id);

            if (expenseCategory == null)
            {
                return NotFound();
            }

            return expenseCategory;
        }

        // PUT: api/ExpenseCategories/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpenseCategory(uint id, ExpenseCategory expenseCategory)
        {
            if (id != expenseCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(expenseCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseCategoryExists(id))
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

        // POST: api/ExpenseCategories
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ExpenseCategory>> PostExpenseCategory(ExpenseCategory expenseCategory)
        {
            _context.ExpenseCategory.Add(expenseCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExpenseCategory", new { id = expenseCategory.Id }, expenseCategory);
        }

        // DELETE: api/ExpenseCategories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ExpenseCategory>> DeleteExpenseCategory(uint id)
        {
            var expenseCategory = await _context.ExpenseCategory.FindAsync(id);
            if (expenseCategory == null)
            {
                return NotFound();
            }

            _context.ExpenseCategory.Remove(expenseCategory);
            await _context.SaveChangesAsync();

            return expenseCategory;
        }

        private bool ExpenseCategoryExists(uint id)
        {
            return _context.ExpenseCategory.Any(e => e.Id == id);
        }
    }
}
