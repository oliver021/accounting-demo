using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OliWorkshop.AccountingSys.Data;

namespace OliWorkshop.AccountingSys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext Context;

        public ExpenseCategoriesController(ApplicationDbContext context)
        {
            Context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseCategory>>> GetExpenseCategory(int page = 0, int length = 45)
        {
            if (page == 0)
            {
                // return all elements
                return await Context.ExpenseCategory.ToListAsync();
            }
            else if (page > 0 && length > 0)
            {
                // return paginate list
                return await Context.ExpenseCategory.Skip(length * (page - 1)).Take(length).ToListAsync();
            }
            else
            {
                // the request is invalid
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseCategory>> GetExpenseCategory(uint id)
        {
            // validate id value
            if (id == default)
            {
                return BadRequest();
            }

            var expenseCategory = await Context.ExpenseCategory.FindAsync(id);

            if (expenseCategory == null)
            {
                return NotFound();
            }

            return expenseCategory;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditName(uint id, string newName)
        {
            // validate id value
            if (id == default)
            {
                return BadRequest();
            }

            Context.Entry(new ExpenseCategory {
                Id = id,
                Name = newName
            }).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // check
                if (! await Context.ExpenseCategory.AnyAsync(e => e.Id == id))
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
        public async Task<ActionResult> PostExpenseCategory(ExpenseCategory expenseCategory)
        {
            // the name category should be unique
            if (await Context.ExpenseCategory.AnyAsync(x => x.Name == expenseCategory.Name))
            {
                return BadRequest("The name category should be unique.");
            }

            Context.ExpenseCategory.Add(expenseCategory);
            await Context.SaveChangesAsync();

            return Ok(new { id = expenseCategory.Id });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExpenseCategory(uint id)
        {
            // validate id value
            if (id == default)
            {
                return BadRequest();
            }

            var expenseCategory = await Context.ExpenseCategory.FindAsync(id);
            if (expenseCategory == null)
            {
                return NotFound();
            }

            Context.ExpenseCategory.Remove(expenseCategory);
            await Context.SaveChangesAsync();

            return NoContent();
        }
    }
}
