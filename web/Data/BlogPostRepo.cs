﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Thyme.Web.Data;

namespace Thyme.Web.Models
{
    public class BlogPostRepo : IDisposable
    {
        StringComparison IgnoreCase = StringComparison.CurrentCultureIgnoreCase;
        CacheState Cache;

        public BlogPostRepo()
        {
            Cache = new CacheState();
        }

        private async Task SetMasterShaToCache()
        {
            var github = new Data.GitHub();
            string masterSha = await github.GetCurrentMasterSha();

            CacheState cache = new CacheState();
            cache.SetCurrentBranchSha(masterSha);
        }

        public string CachedSha
        {
            get
            {
                CacheState cache = new CacheState();
                return cache.GetCurrentBranchSha();
            }
        }

        /// <summary>
        /// Gethe the blog post from cache. First try ASP.NET HttpCache, then disk. If not there, then 404.
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        public BlogPost GetPost(string slug)
        {
            try
            {
                var blogPost = GetBlogPostFromHttpCache(slug);
                if (blogPost == null)
                {
                    //load the post from disk and into the cache.
                    TryLoadDiskItemToCache(slug);
                    blogPost = GetBlogPostFromHttpCache(slug);
                }
                return blogPost;
            }
            catch (Exception)
            {
                //this really should be a 404.
                return new BlogPost { Title = "Oops", Body = "Ya... that either isn't a blog post, or we can't load it right now. So sorry." };
            }
        }

        public void TryRebuildHttpCacheFromDisk()
        {
            var localFileCache = new LocalFileCache();
            var blogPostsFromDisk = new List<BlogPost>();
            foreach (var blogFileOnDisk in localFileCache.ListItemsOnDisk())
            {
                BlogPost blogPost = BlogPostParsing.ConvertFileToBlogPost(blogFileOnDisk.Key, blogFileOnDisk.Value);
                blogPostsFromDisk.Add(blogPost);
            }
            Cache.Clear();
            Cache.AddPostsToCache(blogPostsFromDisk);
        }

        private void TryLoadDiskItemToCache(string slug)
        {
            var localFileCache = new LocalFileCache();
            FileInfo postOnDisk = localFileCache.GetItemOnDisk(slug);
            string fileContents = new LocalFileCache().ReadFileContents(postOnDisk.Name);

            var blogPost = BlogPostParsing.ConvertFileToBlogPost(postOnDisk.Name, fileContents);

            var cache = new CacheState();
            cache.AddPostsToCache(new[] { blogPost });
        }

        private BlogPost GetBlogPostFromHttpCache(string slug)
        {
            return Cache.GetCachedPosts().SingleOrDefault(x => x.UrlSlug.Equals(slug, IgnoreCase));
        }

        public async Task RefreshCachedBlogPosts()
        {
            var github = new Data.GitHub();
            var blogPosts = await github.GetAllBlogPosts();
            Cache.AddPostsToCache(blogPosts );
            await SetMasterShaToCache();
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

        public IQueryable<BlogPost> PublishedPosts
        {
            get
            {
                if (Cache.GetCachedPosts().IsEmpty())
                {
                    TryRebuildHttpCacheFromDisk();
                }

                return Cache.GetCachedPosts().Where(x => x.PublishedOn.GetValueOrDefault(DateTime.MaxValue) <= DateTime.UtcNow);
            }
        }

        public void Dispose() { }
    }
}