using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Thyme.Web.Helpers
{
    public static class Config
    {


        /// <summary>
        /// Gets the number of posts to show on the front page of the blog site. If any exception occurs, 
        /// a large number will be returned (int.maxvalue) and allow basically all posts to be shown.
        /// </summary>
        public static int NumPostsFrontPage
        {
            get
            {
                try
                {
                    int numPosts = int.Parse(ConfigurationManager.AppSettings["FrontPageShowNumberPosts"].ToString());
                    return (numPosts > 0) ? numPosts : int.MaxValue;
                }
                catch (Exception) { return int.MaxValue; }
            }
        }

        public static double CacheTTLHours { get { return double.Parse(ConfigurationManager.AppSettings["RepoTTLInCacheHours"]); } }
        public static string BlogName { get { return  ConfigurationManager.AppSettings["BlogSiteName"]; } }

        public static string GitHubRepo { get { return ConfigurationManager.AppSettings["GitHubRepo"]; } }
        public static string GitHubOwner { get { return ConfigurationManager.AppSettings["GitHubOwner"]; } }

        /// <summary>
        /// Gets the datetime format from config. If config setting is missing, or any exception, the default MMM dd YYYY format will be returned.
        /// </summary>
        public static string DateTimeFormat
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["DateTimeFormat"].ToString();
                }
                catch (Exception) { return "MMM dd YYYY"; }
            }
        }
    }
}