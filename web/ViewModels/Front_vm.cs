using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thyme.Web.Models;

namespace Thyme.Web.ViewModels
{
    public class Front_vm
    {
        public IEnumerable<BlogPost> RecentBlogPosts { get; set; }
    }
}