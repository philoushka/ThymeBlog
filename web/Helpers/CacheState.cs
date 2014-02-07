using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Thyme.Web.Helpers;

namespace Thyme.Web.Models
{
    public class CacheState
    {
        private const string BlogPosts = "BlogPosts";
        private const string MasterSha = "MasterSha";

        public void PutPostsToCache(IEnumerable<BlogPost> posts)
        {            
            HttpRuntime.Cache[BlogPosts] = posts.ToList();      
        }

        public IEnumerable<BlogPost> GetCachedPosts()
        {
            try
            {
                if (HttpRuntime.Cache.Get(BlogPosts) != null)
                {
                    return (HttpRuntime.Cache.Get(BlogPosts) as List<BlogPost>);
                }
            }
            catch (Exception) { }
            return Enumerable.Empty<BlogPost>();
        }

        public string GetCurrentBranchSha()
        {
            try
            {
                if (HttpRuntime.Cache[MasterSha] != null)
                {
                    return (HttpRuntime.Cache[MasterSha] as string);
                }
            }
            catch (Exception) { }
            return string.Empty;
        }

        public void SetCurrentBranchSha(string sha)
        {
            HttpRuntime.Cache[MasterSha] = sha;            
        }

    }
}