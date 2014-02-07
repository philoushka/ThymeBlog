
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Thyme.Web;

namespace Thyme.Web.Models
{
    public class BlogPostRepo : IDisposable
    {
        StringComparison IgnoreCase = StringComparison.CurrentCultureIgnoreCase;
        CacheState Cache;

        public BlogPostRepo()
        {
            Cache = new CacheState();

            if (PublishedPosts.IsEmpty() || IsMasterBranchShaMismatch())
            {
                RefreshCachedBlogPosts();
                SetMasterShaToCache();
            }
        }

        private void SetMasterShaToCache()
        {
            CacheState cache = new CacheState();
            var github = new Data.GitHub();
            cache.SetCurrentBranchSha(github.GetCurrentMasterSha());
        }

        public bool IsMasterBranchShaMismatch()
        {
            CacheState cache = new CacheState();
            var github = new Data.GitHub();
            return (cache.GetCurrentBranchSha() != github.GetCurrentMasterSha());
        }
        public BlogPost GetPost(string slug)
        {
            try
            {
                return Cache.GetCachedPosts().SingleOrDefault(x => x.UrlSlug.Equals(slug, IgnoreCase));
            }
            catch (InvalidOperationException) { throw new System.Web.HttpException(404, "That blog post can't be found right now."); }
            catch (Exception)
            {
                return null;
            }
        }

        public void RefreshCachedBlogPosts()
        {
            var github = new Data.GitHub();
            var blogPosts = github.GetAllBlogPosts();
            Cache.PutPostsToCache(blogPosts);
        }

        public IEnumerable<BlogPost> ListRecentBlogPosts(int numToTake)
        {
            return PublishedPosts
                    .OrderByDescending(x => x.PublishedOn)
                    .Take(numToTake);
        }

        public IEnumerable<BlogPost> SearchPosts(string[] keywords)
        {
            return PublishedPosts
                    .Where(x => keywords.Any(k => x.Body.Contains(k, IgnoreCase)))
                    .OrderByDescending(x => x.PublishedOn);
        }

        public IEnumerable<BlogPost> PublishedPosts
        {
            get
            {
                return Cache.GetCachedPosts().Where(x => x.PublishedOn.HasValue && x.PublishedOn.Value <= DateTime.UtcNow.Date);
            }
        }

        public void Dispose() { }
    }
}