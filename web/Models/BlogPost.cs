using System;

namespace Thyme.Web.Models
{
    public class BlogPost
    {
        public DateTime CreatedOn { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string UrlSlug { get; set; }
        public string Intro { get; set; }
        public string FileName { get; set; }
        public string SHA { get; set; }
        public string Url { get; set; }
    }

    public class BlogPostMetaProperties
    {
        public string Title { get; set; }
        public string PublishedOn { get; set; }
        public string Intro { get; set; }
    }
}