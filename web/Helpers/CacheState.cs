using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        public void AddPostsToCache(IEnumerable<BlogPost> posts)
        {
            var existingPosts = GetCachedPosts().ToList();
            existingPosts.AddRange(posts);
            PutPostsToCache(existingPosts);
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

        public void RemovePostsByName(IEnumerable<string> foo)
        {
            var existingPosts = GetCachedPosts().ToList();
            var fileNames = foo.ToList();
            existingPosts.RemoveAll(x => fileNames.Contains(x.FileName));
            PutPostsToCache(existingPosts);
        }
    }
}