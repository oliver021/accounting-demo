using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OliWorkshop.AccountingSys.Data;
using Microsoft.EntityFrameworkCore;
using OliWorkshop.AccountingSys.Models;
using Microsoft.AspNetCore.Authorization;
using OliWorkshop.AccountingSys.Components;
using System.Security.Claims;

namespace OliWorkshop.AccountingSys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class GroupsController : ControllerBase
    {
        private ApplicationDbContext Context;
        private readonly GroupsService GroupService;

        public GroupsController(ApplicationDbContext context, GroupsService service)
        {
            Context = context;
            GroupService = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Fetch full list or paginate records
        /// and show the categories number
        /// </summary>
        /// <param name="page"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountableGroup>>> GetGroups(int page = 0, int length = 45)
        {
            // count by selection and send the json result
            try
            {
                return Ok(await GroupService.GetGroupWithMetrics(HttpContext.User.GetUserId(), page, length));
            }
            catch (BadPaginationException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get a only record by identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CountableGroup>> GetGroups(uint id)
        {
            // validate id value
            if (id == default)
            {
                return BadRequest();
            }

            var groups = await Context.CountableGroup.FindAsync(id);

            if (groups == null)
            {
                return NotFound();
            }

            return groups;
        }

        /// <summary>
        /// EditName is endpoint to change a name of group
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Groups"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditName(uint id, string newName)
        {
            // validate id value
            if (id == default)
            {
                return BadRequest();
            }

            // create target record
            var current = new CountableGroup();
            current.Id = id;
            current.Name = newName;

            // mark as modified
            Context.Entry(current).State = EntityState.Modified;

            try
            {
                // try commit change
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // check problems
                if (! await Context.CountableGroup.AnyAsync(e => e.Id == id))
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

        /// <summary>
        /// Create a new group record
        /// </summary>
        /// <param name="Groups"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> PostGroups(CountableGroup Groups)
        {
            // add new group as added
            Context.CountableGroup.Add(Groups);

            // commit a new group
            await Context.SaveChangesAsync();

            // set std status code for this action
            HttpContext.Response.StatusCode = 201;

            // return a new id for group
            return Ok( new { id = Groups.Id });
        }

        [HttpPost("{groupId}/Members")]
        public async Task<ActionResult> ManageMembers(uint groupId, 
            [FromBody] List<MemberAction> memberActions)
        {
            // validate id value
            if (groupId == default || memberActions.Count() < 1)
            {
                return BadRequest();
            }

            // loop execution
            foreach (var action in memberActions)
            {
                if (action.IsIncome)
                {
                    if (action.Remove)
                    {
                        var current = new CategoryEarnGroup
                        {
                            Id = action.Target
                        };
                        Context.Remove(current);
                    }
                    else
                    {
                        var current = new CategoryEarnGroup { 
                            EarnCategoryId = action.Target,
                            CountableGroupId = groupId
                        };

                        Context.Add(current);
                    }
                }
                else
                {
                    if (action.Remove)
                    {
                        var current = new CategoryExpenseGroup
                        {
                            Id = action.Target
                        };
                        Context.Remove(current);
                    }
                    else
                    {
                        var current = new CategoryExpenseGroup
                        {
                            ExpenseCategoryId = action.Target,
                            CountableGroupId = groupId
                        };

                        Context.Add(current);
                    }
                }
            }

            // commit changes
            await Context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<NoContentResult>> DeleteGroups(uint id)
        {
            // validate id value
            if (id == default)
            {
                return BadRequest();
            }

            // find the target group
            var Groups = await Context.CountableGroup.FindAsync(id);
            
            // check that the target group exists
            if (Groups == null)
            {
                return NotFound();
            }

            Context.CountableGroup.Remove(Groups);

            // commit that the taget group is deleted
            await Context.SaveChangesAsync();

            return NoContent();
        }
    }
}
