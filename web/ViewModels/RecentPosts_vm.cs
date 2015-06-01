using System;
using System.Collections.Generic;

namespace Thyme.Web.ViewModels
{
    public class RecentPosts_vm
    {
        public IEnumerable<RecentPost> RecentBlogPosts { get; set; }


    }

    public class RecentPost
    {
        public DateTime PostedOnDateTime { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
    }
}