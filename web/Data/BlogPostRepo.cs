using NGit.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Thyme.Web.Models
{
    public class BlogPostRepo : IDisposable
    {
        Git Repo;
        public void Dispose()
        {
            Repo.GetRepository().Close();
            Repo.GetRepository().ObjectDatabase.Close();
            Repo = null;
        }

        public BlogPostRepo()
        {
            try
            {
                if (RepoIsOnDisk() == false)
                {
                    CloneRepo();
                }
                Repo = Git.Open(LocalRepoPath);

                RefreshRepoIfReqd();
            }
            catch (NGit.Errors.RepositoryNotFoundException) { CloneRepo(); }
        }


        public string GitRepoUri { get { return ConfigurationManager.AppSettings["GitRepo"].ToString(); } }

        public string LocalRepoPath { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["BlogFilesDir"].ToString()); } }
        public BlogPost GetPost(string slug)
        {
            try
            {
                var p = new DirectoryInfo(LocalRepoPath).EnumerateFiles("*.md", SearchOption.AllDirectories).Single(x => x.Name == "{0}.md".FormatWith(slug));
                return ConvertFileToBlogPost(p);
            }
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

        public void RefreshRepo()
        {
            try
            {
                PullRepo();
            }
            catch (Exception)
            {
                CloneRepo();
            }
        }

        public IEnumerable<BlogPost> ListRecentBlogPosts(int numToTake)
        {
            var allMarkDowns = new DirectoryInfo(LocalRepoPath).EnumerateFiles("*.md", SearchOption.TopDirectoryOnly);
            
            var recentMarkdowns = ConvertMarkdownsToBlogPosts(allMarkDowns)
                                  .Where(x => x.PublishedOn.HasValue)
                                  .OrderByDescending(x => x.PublishedOn)
                                  .Take(numToTake);

            return recentMarkdowns;
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
                //   Body = File.ReadAllText(file.FullName)
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
            CacheState.LastRepoRefreshDate = DateTime.UtcNow;
        }

        private void PullRepo()
        {
            Repo.Pull().Call();
            CacheState.LastRepoRefreshDate = DateTime.UtcNow;
        }
    }
}