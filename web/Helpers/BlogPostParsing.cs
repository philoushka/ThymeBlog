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
                if (input.HasValue())
                {
                    string metaComment = BlogPostParsing.ExtractMetaComment(input);

                    metaComment = metaComment.Trim().TrimStart("<!--").TrimEnd("-->");
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<BlogPostMetaProperties>(metaComment);
                }
            }
            catch (Exception) { }
            return new BlogPostMetaProperties { Title = "Blog Post", Intro = "Blog Intro", PublishedOn = DateTime.Now.ToString() };
        }

        public static string ExtractMetaComment(string fileContents)
        {
            List<string> lines = ConvertStringToLines(fileContents).ToList();

            if (StringIsHtmlComment(lines.First()))
            {
                return lines.First();
            }
            return string.Empty;
        }

        public static string RemovePostHeader(string fileContents)
        {
            if (fileContents.IsNullorEmpty()) return string.Empty;

            List<string> lines = ConvertStringToLines(fileContents).ToList();

            if (StringIsHtmlComment(lines.First()))
            {
                lines.RemoveAt(0);
            }

            return string.Join(Environment.NewLine, lines);
        }

        public static IEnumerable<string> ConvertStringToLines(string input)
        {
            return input.Trim().Split(Environment.NewLine.ToCharArray()).ToList();
        }

        public static bool StringIsHtmlComment(string input)
        {
            return (input.StartsWith("<!--") && input.EndsWith("-->"));

        }
    }
}