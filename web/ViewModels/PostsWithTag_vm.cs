using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thyme.Web.Models;

namespace Thyme.Web.ViewModels
{
    public class PostsWithTag_vm
    {
        public IEnumerable<BlogPost> Posts { get; set; }
        public string Tag { get; set; }
    }
}