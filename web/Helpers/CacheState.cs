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

        private void PutPostsToHttpCache(IEnumerable<BlogPost> posts)
        {
            HttpRuntime.Cache[BlogPosts] = posts.ToList();
        }

        public void AddPostsToCache(IEnumerable<BlogPost> posts)
        {
            var existingPosts = GetCachedPosts().ToList();
            existingPosts.AddRange(posts);
            PutPostsToHttpCache(existingPosts);
        }

        public IQueryable<BlogPost> GetCachedPosts()
        {
            try
            {
                if (HttpRuntime.Cache.Get(BlogPosts) != null)
                {
                    return (HttpRuntime.Cache.Get(BlogPosts)as List<BlogPost>).AsQueryable();
                }
            }
            catch (Exception) { }
            return Enumerable.Empty<BlogPost>().AsQueryable();
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

        public void RemovePostsByName(IEnumerable<string> postNamesToRemove)
        {
            var existingPosts = GetCachedPosts().ToList();
            var fileNames = postNamesToRemove.ToList();
            existingPosts.RemoveAll(x => fileNames.Contains(x.FileName));
            PutPostsToHttpCache(existingPosts);
        }

        public void Clear() {
            HttpRuntime.Cache.Remove(BlogPosts);
        }
    }
}