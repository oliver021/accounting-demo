using Microsoft.EntityFrameworkCore;
using OliWorkshop.AccountingSys.Data.Models;
using OliWorkshop.AccountingSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using System.Globalization;

namespace OliWorkshop.AccountingSys.Helpers
{
    /// <summary>
    /// This class contains many helpers methods related with data logic
    /// </summary>
    public static class DataHelpers
    {
        public static async Task ResolveResume(IQueryable<IAssetAnotation> query, Resume resume)
        {

            resume.Total = await query.SumAsync(x => x.Amount);
            resume.AvarageTotal = await query.AverageAsync(x => x.Amount);

            resume.AvaragePerYear = await query
                .GroupBy(x => x.AtCreated.Year)
                .Select(x => x.Sum(j => j.Amount))
                .AverageAsync();

            resume.AvaragePerMonth = await query
               .GroupBy(x => x.AtCreated.Month)
               .Select(x => x.Sum(j => j.Amount))
               .AverageAsync();

            resume.AvaragePerDay = await query
               .GroupBy(x => x.AtCreated.Day)
               .Select(x => x.Sum(j => j.Amount))
               .AverageAsync();
        }

        /// <summary>
        /// Prepare an record to humanize yours properties with texts
        /// </summary>
        /// <param name="asset"></param>
        public static void Humanize(IAssetAnotation asset, CultureInfo culture = null)
        {
            if (asset is null)
            {
                throw new ArgumentNullException(nameof(asset));
            }

            asset.TextDate = asset.AtCreated.ToOrdinalWords();
            asset.TextDateAgo = asset.AtCreated.Humanize(true, DateTime.Now, culture);
        }
    }
}
