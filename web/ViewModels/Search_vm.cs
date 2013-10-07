using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thyme.Web.Models;

namespace Thyme.Web.ViewModels
{
    public class Search_vm
    {
        public string SearchKeywords { get; set; }
        public IEnumerable<BlogPost> MatchingBlogPosts { get; set; }
    }
}