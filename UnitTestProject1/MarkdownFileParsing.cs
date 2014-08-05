using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thyme.Web.Helpers;
using System.Text;
using Thyme.Web;
using Thyme.Web.Models;
namespace Thyme.Tests
{
    [TestClass]
    public class MarkdownFileParsing
    {
        const string MarkdownBlogpostEmptyHeader = @"<!--{{Title:'{0}',Intro:'{1}',PublishedOn:'{2}'}}-->";

        [TestMethod]
        public void CanParseGoodHeaderComment()
        {
            const string GoodBody = "body post abc foo bar";
            const string GoodTitle = "some title";
            DateTime blogpostDate = DateTime.Now;
            string goodDate = blogpostDate.ToString("dd-MMM-yyyy");
            const string GoodIntro = "some intro";
            StringBuilder goodBlogpost = new StringBuilder();
            goodBlogpost.AppendLine(string.Format(MarkdownBlogpostEmptyHeader, GoodTitle, GoodIntro, goodDate));
            goodBlogpost.AppendLine(GoodBody);

            string metaComment = BlogPostParsing.ExtractMetaComment(goodBlogpost.ToString());

            metaComment = metaComment.Trim().TrimStart("<!--").TrimEnd("-->");
            BlogPostMetaProperties meta = Newtonsoft.Json.JsonConvert.DeserializeObject<BlogPostMetaProperties>(metaComment);

            Assert.AreEqual(meta.Intro, GoodIntro);
            Assert.AreEqual(meta.Title, GoodTitle);
            Assert.AreEqual(meta.PublishedOn, goodDate);


        }


        [TestMethod]
        public void CanParseHeaderCommentWithDateTime()
        {
            const string GoodBody = "body post abc foo bar";
            const string GoodTitle = "some title";
            DateTime blogpostDate = DateTime.Now;
            string goodDate = blogpostDate.ToString("dd-MMM-yyyy HH:mm");
            const string GoodIntro = "some intro";
            StringBuilder goodBlogpost = new StringBuilder();
            goodBlogpost.AppendLine(string.Format(MarkdownBlogpostEmptyHeader, GoodTitle, GoodIntro, goodDate));
            goodBlogpost.AppendLine(GoodBody);

            string metaComment = BlogPostParsing.ExtractMetaComment(goodBlogpost.ToString());
            var meta = BlogPostParsing.ParseValuesFromComment(metaComment);
             
            Assert.AreEqual(meta.PublishedOn, goodDate);

        }

    }
}
