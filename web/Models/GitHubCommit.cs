using System;
using System.Collections.Generic;
using System.Linq;

namespace Thyme.Web.Models
{
    public class GitHubPostedCommit
    {
        public string after { get; set; }
        public IEnumerable<GitHubCommit> commits { get; set; }


        public IEnumerable<string> RemovedPosts
        {
            get
            {
                try
                {
                    return this.commits.First().removed;
                }
                catch (Exception)
                {
                    return Enumerable.Empty<string>();
                }
            }
        }
    }
    public class GitHubCommit
    {

        public string id { get; set; }
        public string url { get; set; }

        public IEnumerable<string> added { get; set; }
        public IEnumerable<string> removed { get; set; }
        public IEnumerable<string> modified { get; set; }

        public IEnumerable<string> ItemsToKeep { get { return added.Union(modified); } }


    }
}