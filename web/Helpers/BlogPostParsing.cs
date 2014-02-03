using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thyme.Web.Models;

namespace Thyme.Web 
{
    public static class BlogPostParsing
    {

        public static BlogPostMetaProperties ParseValuesFromComment(string input)
        {
            try
            {
                input = input.Replace("<!--", string.Empty).Replace("-->", string.Empty);
                if (input.HasValue())
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<BlogPostMetaProperties>(input);
                else
                    return new BlogPostMetaProperties { Title = "Blog Post", Intro = "Blog Intro", PublishedOn = DateTime.Now.ToString() };
            }
            catch (Exception) { }
            return new BlogPostMetaProperties { Title = "Blog Post", Intro = "Blog Intro", PublishedOn = DateTime.Now.ToString() };
        }



    }
}