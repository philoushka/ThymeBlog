﻿using Thyme.Web.Models;

namespace Thyme.Web.ViewModels
{
    public class BlogPost_vm
    {
        public BlogPost BlogPost { get; set; }
        public string WriteTags()
        {
            return string.Join(", ", BlogPost.Tags);
        }
    }
}