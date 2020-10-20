using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using OliWorkshop.AccountingSys.Data;
using OliWorkshop.AccountingSys.Models;
using OliWorkshop.AccountingSys.Helpers;

namespace OliWorkshop.AccountingSys.Pages.Earns
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Earn> Earn { get;set; }
        public List<RecordGroupDate> RecordGroupDate { get;set; }
        public string GroupRecords {get; set; } = string.Empty;
        public IEnumerable<SelectListItem> SelectCategories { get; private set; }

        public Pagination Paginate { get; set; }

        public async Task OnGetAsync()
        {
            var noneCategory = new SelectListItem("Ninguna", "none");
            SelectCategories = new SelectList(
            await _context.EarnCategory
            .Where(x => x.UserId == HttpContext.User.GetUserId()).ToListAsync(), "Id", "Name", noneCategory)
            .Concat(new List<SelectListItem> {noneCategory})
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

            if (groupType != StringValues.Empty && groupType != "none")
            {
                switch (groupType)
                {

                    case "day":
                            GroupRecords = "Day";
                            var result = await query
                            .GroupBy(x => x.AtCreated.Day)
                            .Select(x => new {
                                x.Key,
                                Amount = x.Sum(j => j.Amount),
                                Count = x.Count()
                            })
                            .ToListAsync();
                        RecordGroupDate = result.Select(x => new RecordGroupDate {
                            Key = x.Key.ToString(),
                            Amount = x.Amount,
                            Count = x.Count
                        }).ToList();
                        break;

                    case "months":
                        GroupRecords = "Month";
                        var result2 = await query
                        .GroupBy(x => x.AtCreated.Month)
                        .Select(x => new {
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
                        .Select(x => new {
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
                        .GroupBy(x => x.EarnCategoryId)
                        .Select(x => new {
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
                        .Select(x => new {
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
            }else{
                Earn = await query.ToListAsync();
                Earn.ForEach(income => DataHelpers.Humanize(income));
            }
        }

        /// <summary>
        /// Crate a smart filter to manage query record from query params
        /// </summary>
        /// <returns></returns>
        private IQueryable<Earn> ResolveQuery()
        {
            var category = HttpContext.Request.Query["category"];
            var term = HttpContext.Request.Query["term"];
            var before = HttpContext.Request.Query["before"];
            var after = HttpContext.Request.Query["after"];
            var amount_min = HttpContext.Request.Query["amount_min"];
            var amount_max = HttpContext.Request.Query["amount_max"];
            IQueryable<Earn> result = _context.Earn;

            if (term != StringValues.Empty)
            {
                result = result.Where(x => x.Concept.Contains(term.ToString()));
            }

            if (category != StringValues.Empty 
                && category != "none" 
                && uint.TryParse(category.ToString(), out uint value))
            {
                result = result.Where(x => x.EarnCategoryId == value);
            }

            if (before != StringValues.Empty && DateTime.TryParse(before, out DateTime instanceBefore))
            {
                result = result.Where(x => x.AtCreated < instanceBefore);
            }

            if (before != StringValues.Empty && DateTime.TryParse(after, out DateTime instanceAfter))
            {
                result = result.Where(x => x.AtCreated > instanceAfter);
            }

            if (amount_min != StringValues.Empty && int.TryParse(amount_min.ToString(), out int instanceMin))
            {
                result = result.Where(x => x.Amount >= instanceMin);
            }

            if (amount_max != StringValues.Empty && int.TryParse(amount_max.ToString(), out int instanceMax))
            {
                result = result.Where(x => x.Amount <= instanceMax);
            }

            return result
                .Where(x => x.UserId == HttpContext.User.GetUserId())
                .OrderByDescending(x => x.AtCreated)
                .Include(e => e.EarnCategory);
        }
    }
}
