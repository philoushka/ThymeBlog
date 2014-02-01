 
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Thyme.Web.Models
{
    public class BlogPostRepo : IDisposable
    {
        StringComparison CaseInsensitive = StringComparison.CurrentCultureIgnoreCase;
        private const string AllMarkdownFiles = "*.md";

        public string GitHubApiUri { get { return ConfigurationManager.AppSettings["GitHubRepoApiUrl"].ToString(); } }

        public string LocalRepoPath { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["BlogFilesDir"].ToString()); } }
  
      

        public BlogPostRepo()
        {
             
        }

      public BlogPost GetPost(string slug)
        {
            try
            {
                var p = new DirectoryInfo(LocalRepoPath).EnumerateFiles("*.md", SearchOption.AllDirectories).Single(x => x.Name == "{0}.md".FormatWith(slug));
                return ConvertFileToBlogPost(p);
            }
            catch (InvalidOperationException) { throw new System.Web.HttpException(404, "That blog post can't be found right now."); }
            catch (Exception)
            {
                return null;
            }
        }

        public void RefreshRepoIfReqd()
        {
            if (CachedRepoIsStale)
            {
                RefreshRepo();
            }
        }
         

       
        private IEnumerable<FileInfo> RepoMarkdownFiles
        {
            get
            {
                return new DirectoryInfo(LocalRepoPath).EnumerateFiles(AllMarkdownFiles, SearchOption.TopDirectoryOnly);
            }
        }
        public IEnumerable<BlogPost> ListRecentBlogPosts(int numToTake)
        {
            return ConvertMarkdownsToBlogPosts(RepoMarkdownFiles)
                                  .Where(x => x.PublishedOn.HasValue)
                                  .OrderByDescending(x => x.PublishedOn)
                                  .Take(numToTake);
        }

        public IEnumerable<BlogPost> SearchPosts(string[] keywords)
        {
            return ConvertMarkdownsToBlogPosts(RepoMarkdownFiles)
                                  .Where(x => x.PublishedOn.HasValue)
                                  .Where(x => keywords.Any(k => x.Body.Contains(k, CaseInsensitive)))
                                  .OrderByDescending(x => x.PublishedOn);
        }

        public IEnumerable<BlogPost> ConvertMarkdownsToBlogPosts(IEnumerable<FileInfo> markdownFiles)
        {
            foreach (var markdown in markdownFiles)
            {
                yield return ConvertFileToBlogPost(markdown);
            }
        }

        public BlogPost ConvertFileToBlogPost(FileInfo file)
        {
            List<string> fileText = File.ReadAllLines(file.FullName).ToList();
            string firstLineComment = fileText.First();
            fileText.RemoveAt(0);
            if (fileText.First().Trim() == string.Empty) { fileText.RemoveAt(0); }
            var metaProps = ParseValuesFromComment(firstLineComment);
            var bp = new BlogPost
            {
                UrlSlug = Path.GetFileNameWithoutExtension(file.FullName),
                CreatedOn = file.CreationTime,
                PublishedOn = (metaProps.PublishedOn.HasValue()) ? DateTime.Parse(metaProps.PublishedOn) : new Nullable<DateTime>(),
                Title = metaProps.Title,
                Intro = metaProps.Intro,
                Body = string.Join(Environment.NewLine, fileText.ToArray())
            };
            return bp;
        }

        private BlogPostMetaProperties ParseValuesFromComment(string input)
        {
            try
            {
                input = input.Replace("<!--", string.Empty).Replace("-->", string.Empty);
                if (input.HasValue())
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<BlogPostMetaProperties>(input);
                else
                    return new BlogPostMetaProperties { Title = "Blog Post", Intro = "Blog Intro", PublishedOn = DateTime.Now.ToString() };
            }
            catch (Exception) { }
            return new BlogPostMetaProperties { Title = "Blog Post", Intro = "Blog Intro", PublishedOn = DateTime.Now.ToString() };

        }

        public bool CachedRepoIsStale
        {
            get
            {
                try { return DateTime.UtcNow.Subtract(CacheState.LastRepoRefreshDate.Value).TotalHours >= long.Parse(ConfigurationManager.AppSettings["RepoTTLInCacheHours"]); }
                catch (Exception) { return true; }
            }
        }

        private bool RepoIsOnDisk()
        {
            return System.IO.Directory.Exists(LocalRepoPath);
        }
        private void CloneRepo()
        {
            if (RepoIsOnDisk())
            {
                System.IO.Directory.CreateDirectory(LocalRepoPath);
            }

            var clone = Git.CloneRepository().SetDirectory(LocalRepoPath).SetURI(GitRepoUri);
            Repo = clone.Call();
            CacheState.LastCommitDate = DateTime.UtcNow;
        }

        private void PullRepo()
        {
            Repo.Pull().Call();
            CacheState.LastRepoRefreshDate = DateTime.UtcNow;
        }
    }
}