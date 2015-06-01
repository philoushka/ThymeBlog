using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thyme.Web.Models;

namespace Thyme.Web.Helpers
{
    public class BlogStatistics
    {
        public static Dictionary<string, string> BuildFooterStats()
        {
            var stats = new Dictionary<string, string>();
            stats.Add("Avg. Monthly Posts", CalcAveragePostPerMonth().ToString("0"));
            stats.Add("Posts In Last 6 Months", NumberPostsLastNMonths().ToString("0"));
            return stats;
        }

        public static int NumberPostsLastNMonths(int monthsPrevious = 6)
        {
            using (var repo = new BlogPostRepo())
            {
                return repo.PublishedPosts.Count(x => x.PublishedOn.Value >= DateTime.UtcNow.AddMonths(-1 * monthsPrevious));
            }
        }
        public static double CalcAveragePostPerMonth()
        {

            using (var repo = new BlogPostRepo())
            {
                return repo.PublishedPosts
                           .Where(x => x.PublishedOn.Value >= DateTime.UtcNow.AddMonths(-11))
                           .GroupBy(x => x.PublishedOn.Value.Month)
                           .Select(g => new
                           {
                               Month = g.Key,
                               NumPosts = g.Count()
                           })
                           .Average(x => x.NumPosts);
            }
        }
    }
}