using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Thyme.Web.Data;
using Thyme.Web.Models;

namespace Thyme.Web
{
    public static class BlogPostParsing
    {
        public static BlogPost ConvertFileToBlogPost(string fileName, string fileContents, string sha = "", string url = "")
        {
            BlogPostMetaProperties metaProps = BlogPostParsing.ParseValuesFromComment(fileContents);
            string urlSlug = Path.GetFileNameWithoutExtension(fileName);
            string body = BlogPostParsing.RemovePostHeader(fileContents);
            string file = Path.GetFileNameWithoutExtension(fileName);
            return new BlogPost
            {
                Body = body,
                FileName = file,
                SHA = sha,
                Url = url,
                Tags= metaProps.Tags??Enumerable.Empty<string>(),
                Title = metaProps.Title,
                Intro = metaProps.Intro,
                PublishedOn = (metaProps.PublishedOn.HasValue()) ? DateTime.Parse(metaProps.PublishedOn) : new Nullable<DateTime>(),
                UrlSlug = urlSlug
            };
        }

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
            return new BlogPostMetaProperties { Title = "Blog Post", Intro = "Blog Intro", PublishedOn = DateTime.Now.ToString()  };
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
            return input.Trim().Replace("\r","").Split(Environment.NewLine.ToCharArray()).ToList();
        }

        public static bool StringIsHtmlComment(string input)
        {
            return (input.StartsWith("<!--") && input.EndsWith("-->"));

        }
    }
}