using OliWorkshop.AccountingSys.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OliWorkshop.AccountingSys.Components
{
    /// <summary>
    /// The main service to work with groups
    /// </summary>
    public class GroupsService
    {
        public GroupsService(ApplicationDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ApplicationDbContext Context { get; }

        /// <summary>
        /// Basic metric for all groups
        /// </summary>
        /// <param name="page"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public async Task<IEnumerable<GroupMetricResult>> GetGroupWithMetrics(
            string userId = "",
            int page = 0, 
            int length = 25)
        {
            // declare query
            IQueryable<CountableGroup> groupsQuery;

            // check pagination feature
            if (page == 0)
            {
                groupsQuery = Context.CountableGroup;
            }
            else if (page > 0 && length > 0)
            {
                groupsQuery = Context.CountableGroup.Skip(length * (page - 1)).Take(length);
            }
            else
            {
                throw new BadPaginationException(ConstantsValues.BadPaginationMessage);
            }

            if (userId != string.Empty)
            {
                groupsQuery = groupsQuery.Where(x => x.UserId == userId);
            }

            // fetch the all categries id for incomes
            var joinQueryEarns = from current in groupsQuery
                                 join expenses in Context.Set<CategoryEarnGroup>()
                                 on current.Id equals expenses.CountableGroupId
                                 join trans in Context.Set<Earn>()
                                 on expenses.EarnCategoryId equals trans.EarnCategoryId into transResult
                                 from record in transResult.DefaultIfEmpty()
                                 select new { current.Id, expenses.EarnCategoryId, record.Amount };

            // fetch the all categries id for expenses
            var joinQueryExpenses = from current in groupsQuery
                                    join expenses in Context.Set<CategoryExpenseGroup>()
                                    on current.Id equals expenses.CountableGroupId
                                    join trans in Context.Set<Expense>()
                                    on expenses.ExpenseCategoryId equals trans.ExpenseCategoryId into transResult
                                    from record in transResult.DefaultIfEmpty()
                                    select new { current.Id, expenses.ExpenseCategoryId, record.Amount };

            // execute results
            var resultCountEarns = await joinQueryEarns.ToArrayAsync();
            var resultCountExpenses = await joinQueryExpenses.ToArrayAsync();

            // fecth the result of group records
            var result = await groupsQuery.ToListAsync();

            // return final results
            return result.Select(x => new GroupMetricResult(
                x.Id,
                x.Name,
                    resultCountEarns
                    .Where(J => J.Id == x.Id)
                    .GroupBy(j => j.EarnCategoryId)
                    .Count(),
                    resultCountExpenses
                    .Where(J => J.Id == x.Id)
                    .GroupBy(j => j.ExpenseCategoryId)
                    .Count(),
                    resultCountEarns
                    .Where(j => x.Id == j.Id)
                    .Count(),
                    resultCountExpenses
                    .Where(j => x.Id == j.Id)
                    .Count(),
                    resultCountEarns
                    .Where(j => x.Id == j.Id)
                    .Sum(j => j.Amount),
                    resultCountExpenses
                    .Where(j => x.Id == j.Id)
                    .Sum(j => j.Amount)
            ));
        }
    }
}
