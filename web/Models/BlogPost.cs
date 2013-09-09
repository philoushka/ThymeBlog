using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


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

    }

    public class BlogPostMetaProperties
    {
        public string Title { get; set; }
        public string PublishedOn { get; set; }

        public string Intro { get; set; }
    }

}