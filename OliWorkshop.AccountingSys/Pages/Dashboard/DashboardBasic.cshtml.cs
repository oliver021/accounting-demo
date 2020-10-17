using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OliWorkshop.AccountingSys.Data;
using OliWorkshop.AccountingSys.Helpers;
using System.Security.Claims;

namespace OliWorkshop.AccountingSys.Pages.Dashboard
{
    public class DashboardBasicModel : PageModel
    {
        public string[] LabelCharts { get; set; }
        public decimal[] TotalIncomes { get; set; }
        public decimal[] TotalExpense { get; set; }

        public DashboardBasicModel(ApplicationDbContext context)
        {
            _context = context;
        }

        private readonly ApplicationDbContext _context;

        public async Task OnGetAsync()
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCulture.RequestCulture.UICulture;
            LabelCharts = MiscHelpers.GetDaysOfWeekFromNow(culture).ToArray();
            var dates = MiscHelpers.GetDateTimesFromBack();

            IQueryable<Earn> queryEarnsBase = _context.Earn
                .Where(x => x.UserId == HttpContext.User.GetUserId());
            IQueryable<Expense> queryExpensesBase = _context.Expense
                .Where(x => x.UserId == HttpContext.User.GetUserId());

            TotalIncomes = new decimal[7];
            TotalExpense = new decimal[7];
            int index = 0;

            foreach (DateTime current in dates)
            {
                TotalIncomes[index] = await queryEarnsBase
                    .Where(x => x.AtCreated.Day == current.Day)
                    .SumAsync(j => j.Amount);

                TotalExpense[index] = await queryExpensesBase
                    .Where(x => x.AtCreated.Day == current.Day)
                    .SumAsync(j => j.Amount);

                index++;
            }
        }
    }
}