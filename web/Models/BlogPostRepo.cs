using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NGit.Api;
using System.Configuration;
using NGit;
using System.IO;
using Thyme.Web;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;

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
                try
                {
                    PullRepo();
                }
                catch (Exception)
                {
                    CloneRepo();
                }
                
            }
        }

        public IEnumerable<BlogPost> ListRecentBlogPosts(int getNumRecent)
        {
            var recentMarkdowns = new DirectoryInfo(LocalRepoPath)
                .EnumerateFiles("*.md", SearchOption.AllDirectories)
                .OrderByDescending(x => x.CreationTime)
                .Take(getNumRecent);

            foreach (var mdFile in recentMarkdowns)
            {
                yield return ConvertFileToBlogPost(mdFile);
            }

        }

        public BlogPost ConvertFileToBlogPost(FileInfo file)
        {
            string[] fileText = File.ReadAllLines(file.FullName);

            string firstLineComment = fileText.First();
            var metaProps = ParseValuesFromComment(firstLineComment);
            var bp = new BlogPost
            {
                UrlSlug = Path.GetFileNameWithoutExtension(file.FullName),
                CreatedOn = file.CreationTime,
                PublishedOn = DateTime.Parse(metaProps.PublishedOn),
                Title = metaProps.Title,
                Intro = metaProps.Intro,
                Body = File.ReadAllText(file.FullName) 
            };
            return bp;
        }

        private BlogPostMetaProperties ParseValuesFromComment(string input)
        {
            input = input.Replace("<!--", string.Empty).Replace("-->", string.Empty);
            if (input.HasValue())
                return Newtonsoft.Json.JsonConvert.DeserializeObject<BlogPostMetaProperties>(input);
            else
                return new BlogPostMetaProperties { Title="Blog Post", Intro="Blog Intro", PublishedOn=DateTime.Now.ToString()  };
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