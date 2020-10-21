using Microsoft.EntityFrameworkCore;
using OliWorkshop.AccountingSys.Data.Models;
using OliWorkshop.AccountingSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using System.Globalization;
using OliWorkshop.AccountingSys.Data;
using Microsoft.Extensions.Primitives;

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

        /// <summary>
        /// Make an evaluation that filter assets
        /// </summary>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <param name="amount_min"></param>
        /// <param name="amount_max"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static IQueryable<IAssetAnotation> FilterAssets(string term, StringValues before, StringValues after, StringValues amount_min, StringValues amount_max, IQueryable<IAssetAnotation> result)
        {

            if (term != StringValues.Empty)
            {
                result = result.Where(x => x.Concept.Contains(term.ToString()));
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

            return result;
        }
    }
}
