using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using OliWorkshop.AccountingSys.Data;
using OliWorkshop.AccountingSys.Helpers;
using OliWorkshop.AccountingSys.Models;

namespace OliWorkshop.AccountingSys.Pages.Expenses
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public List<RecordGroupDate> RecordGroupDate { get; set; }
        public string GroupRecords { get; set; } = string.Empty;
        public IEnumerable<SelectListItem> SelectCategories { get; private set; }

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Expense> Expense { get;set; }

        /// <summary>
        /// Resolve data when the get is requested
        /// </summary>
        /// <returns></returns>
        public async Task OnGetAsync()
        {
            var noneCategory = new SelectListItem("Ninguna", "none");
            SelectCategories = new SelectList(
            await _context.ExpenseCategory
            .Where(x => x.UserId == HttpContext.User.GetUserId()).ToListAsync(), "Id", "Name", noneCategory)
            .Concat(new List<SelectListItem> { noneCategory })
            .Reverse();

            await MakeQuery();
        }


        /// <summary>
        /// Make a query base on predefine number of query parameters
        /// </summary>
        /// <returns></returns>
        private async Task MakeQuery()
        {
            var query = ResolveQuery();

            var groupType = HttpContext.Request.Query["grouping"];
            await GroupingIfIsRequested(query, groupType);
        }

        /// <summary>
        /// If the group is requested
        /// </summary>
        /// <param name="query"></param>
        /// <param name="groupType"></param>
        /// <returns></returns>
        private async Task GroupingIfIsRequested(IQueryable<Expense> query, StringValues groupType)
        {
            if (groupType != StringValues.Empty && groupType != "none")
            {
                await ResolveGroup(query, groupType);
            }
            else
            {
                Expense = await query.ToListAsync();
                Expense.ForEach(income => DataHelpers.Humanize(income));
            }
        }

        /// <summary>
        /// The group resolver
        /// </summary>
        /// <param name="query"></param>
        /// <param name="groupType"></param>
        /// <returns></returns>
        private async Task ResolveGroup(IQueryable<Expense> query, StringValues groupType)
        {
            switch (groupType)
            {

                case "day":
                    GroupRecords = "Day";
                    var result = await query
                    .GroupBy(x => x.AtCreated.Day)
                    .Select(x => new
                    {
                        x.Key,
                        Amount = x.Sum(j => j.Amount),
                        Count = x.Count()
                    })
                    .ToListAsync();
                    RecordGroupDate = result.Select(x => new RecordGroupDate
                    {
                        Key = x.Key.ToString(),
                        Amount = x.Amount,
                        Count = x.Count
                    }).ToList();
                    break;

                case "months":
                    GroupRecords = "Month";
                    var result2 = await query
                    .GroupBy(x => x.AtCreated.Month)
                    .Select(x => new
                    {
                        x.Key,
                        Amount = x.Sum(j => j.Amount),
                        Count = x.Count()
                    })
                    .ToListAsync();
                    RecordGroupDate = result2.Select(x => new RecordGroupDate
                    {
                        Key = x.Key.ToString(),
                        Amount = x.Amount,
                        Count = x.Count
                    }).ToList();
                    break;

                case "year":
                    GroupRecords = "Year";
                    var result3 = await query
                    .GroupBy(x => x.AtCreated.Year)
                    .Select(x => new
                    {
                        x.Key,
                        Amount = x.Sum(j => j.Amount),
                        Count = x.Count()
                    })
                    .ToListAsync();
                    RecordGroupDate = result3.Select(x => new RecordGroupDate
                    {
                        Key = x.Key.ToString(),
                        Amount = x.Amount,
                        Count = x.Count
                    }).ToList();
                    break;

                case "category":
                    GroupRecords = "Category";
                    var result4 = await query
                    .GroupBy(x => x.ExpenseCategoryId)
                    .Select(x => new
                    {
                        x.Key,
                        Amount = x.Sum(j => j.Amount),
                        Count = x.Count()
                    })
                    .ToListAsync();
                    RecordGroupDate = result4.Select(x => new RecordGroupDate
                    {
                        Key = SelectCategories.First(j => j.Value == x.Key.ToString()).Text,
                        Amount = x.Amount,
                        Count = x.Count
                    }).ToList();
                    break;

                case "concepts":
                    GroupRecords = "Concepts";
                    var result5 = await query
                    .GroupBy(x => x.Concept)
                    .Select(x => new
                    {
                        x.Key,
                        Amount = x.Sum(j => j.Amount),
                        Count = x.Count()
                    })
                    .ToListAsync();
                    RecordGroupDate = result5.Select(x => new RecordGroupDate
                    {
                        Key = x.Key.ToString(),
                        Amount = x.Amount,
                        Count = x.Count
                    }).ToList();
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Crate a smart filter to manage query record from query params
        /// </summary>
        /// <returns></returns>
        private IQueryable<Expense> ResolveQuery()
        {
            var category = HttpContext.Request.Query["category"];
            var term = HttpContext.Request.Query["term"];
            var before = HttpContext.Request.Query["before"];
            var after = HttpContext.Request.Query["after"];
            var amount_min = HttpContext.Request.Query["amount_min"];
            var amount_max = HttpContext.Request.Query["amount_max"];
            IQueryable<Expense> result = _context.Expense;


            if (category != StringValues.Empty
                && category != "none"
                && uint.TryParse(category.ToString(), out uint value))
            {
                result = result.Where(x => x.ExpenseCategoryId == value);
            }

            var result2 = DataHelpers.FilterAssets(term, before, after, amount_min, amount_max, result);

            return result2.Cast<Expense>()
                .Where(x => x.UserId == HttpContext.User.GetUserId())
                .OrderByDescending(x => x.AtCreated)
                .Include(e => e.ExpenseCategory);
        }
    }
}
