using System;
using System.Collections.Generic;
using System.Linq;

namespace Thyme.Web.Models
{
    public class BlogPost
    {
        public BlogPost()
        {
            Tags = Enumerable.Empty<string>();
        }
        public DateTime? PublishedOn { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string UrlSlug { get; set; }
        public string Intro { get; set; }
        public string FileName { get; set; }
        public string SHA { get; set; }
        public string Url { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }

    public class BlogPostMetaProperties
    {
        public string Title { get; set; }
        public string PublishedOn { get; set; }
        public string Intro { get; set; }
        public IEnumerable<string> Tags { get; set; }

    }
}