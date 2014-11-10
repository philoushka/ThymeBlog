using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thyme.Web.Data;

namespace Thyme.Web.Models
{
    public class BlogPostRepo : IDisposable
    {
        StringComparison IgnoreCase = StringComparison.CurrentCultureIgnoreCase;

        public BlogPostRepo() { }

        public IEnumerable<BlogPost> PublishedPosts { get { return ListBlogPostsOnDisk().Where(x => x.PublishedOn.HasValue && x.PublishedOn <= DateTime.UtcNow); } }

        /// <summary>
        /// Get the the blog post from disk 
        /// </summary>
        public BlogPost GetPost(string slug)
        {
            try
            {
                var blogPost = PublishedPosts.Single(x => x.UrlSlug == slug);
                return blogPost;
            }
            catch (Exception)
            {
                //this really should be a 404.
                return new BlogPost { Title = "Oops", Body = "Ya... that either isn't a blog post, or we can't load it right now. So sorry." };
            }
        }

        /// <summary>
        /// Get all blog posts on disk.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BlogPost> ListBlogPostsOnDisk()
        {
            var localFileCache = new LocalFileCache();

            foreach (var blogFileOnDisk in localFileCache.ListItemsOnDisk())
            {
                yield return BlogPostParsing.ConvertFileToBlogPost(blogFileOnDisk.Key, blogFileOnDisk.Value);
            }
        }

        /// <summary>
        /// Retrieve all posts from the Git repo, and save to disk.
        /// </summary>        
        public async Task RefreshCachedBlogPosts()
        {
            var github = new Data.GitHub();
            await github.SaveAllBlogPostsFromGitHubToDisk();
        }

        /// <summary>
        /// Get the *n* most recent blog posts by date.
        /// </summary>        
        public IEnumerable<BlogPost> ListRecentBlogPosts(int numToTake)
        {
            return PublishedPosts
                    .OrderByDescending(x => x.PublishedOn)
                    .Take(numToTake);
        }

        /// <summary>
        /// Find posts on disk that contain the keywords supplied
        /// </summary>        
        public IEnumerable<BlogPost> SearchPosts(string[] keywords)
        {
            return PublishedPosts
                    .Where(x => keywords.Any(k => x.Body.Contains(k, IgnoreCase)))
                    .OrderByDescending(x => x.PublishedOn);
        }



        public void Dispose() { }
    }
}