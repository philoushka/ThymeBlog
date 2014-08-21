using System;
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
                catch (Exception) { }
                return int.MaxValue;
            }
        }

        public static string TwitterAcct { get { return ConfigurationManager.AppSettings["TwitterAcct"]; } }
        public static string GoogleAnalyticsAccountNumber { get { return ConfigurationManager.AppSettings["GoogleAnalyticsAccountNumber"]; } }
        public static string BlogName { get { return ConfigurationManager.AppSettings["BlogSiteName"]; } }
        public static string GitHubRepo { get { return ConfigurationManager.AppSettings["GitHubRepo"]; } }
        public static string GitHubOwner { get { return ConfigurationManager.AppSettings["GitHubOwner"]; } }

        public static string MyName { get { return ConfigurationManager.AppSettings["MyName"]; } }

        public static string StackOverflowUserNumber { get { return ConfigurationManager.AppSettings["StackOverflowUserNumber"]; } }

        public static int OutputCacheSeconds { get { return int.Parse(ConfigurationManager.AppSettings["OutputCacheSeconds"]); } }

        public static string GitHubOAuthToken { get { return ConfigurationManager.AppSettings["GitHubOAuthToken"]; } }

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

        public static string BlogFilesDir { get { return ConfigurationManager.AppSettings["BlogFilesDir"]; } }
    }
}