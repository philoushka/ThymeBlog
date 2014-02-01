using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Thyme.Web.Helpers;

namespace Thyme.Web.Models
{
    public static class CacheState
    {
        private const string CommitDate = "LhDate";
        private const string BlogPosts = "BlogPosts";
        public static DateTime? LastCommitDate
        {
            get
            {
                DateTime? lastCommit = null;
                if (HttpRuntime.Cache[CommitDate] != null)
                {
                    lastCommit = new DateTime(Convert.ToInt64(HttpRuntime.Cache[CommitDate]));
                }

                return lastCommit;
            }
            set { HttpRuntime.Cache[CommitDate] = value.Value.Ticks; }
        }


        public void PutPostsToCache(IEnumerable<BlogPost> posts)
        {

            HttpRuntime.Cache.Add(BlogPosts, posts, null, DateTime.Now.AddHours(Config.CacheTTLHours), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);

        }

        public IEnumerable<BlogPost> GetCachedPosts()
        {
            try
            {
                return (HttpRuntime.Cache[BlogPosts] as IEnumerable<BlogPost>);
            }
            catch (Exception) {
                return new List<BlogPost>();
            }
        }
    }
}