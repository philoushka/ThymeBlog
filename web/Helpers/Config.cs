using System;
using System.Collections.Generic;
using System.Configuration;

namespace Thyme.Web.Helpers
{
    public static class Config
    {
        public static string TwitterAcct => ConfigurationManager.AppSettings[nameof(TwitterAcct)];
        public static string GoogleAnalyticsAccountNumber => ConfigurationManager.AppSettings[nameof(GoogleAnalyticsAccountNumber)];
        public static string GooglePlusAccountNumber => ConfigurationManager.AppSettings[nameof(GooglePlusAccountNumber)];
        public static string BlogSiteName => ConfigurationManager.AppSettings[nameof(BlogSiteName)];
        public static string GitHubRepo => ConfigurationManager.AppSettings[nameof(GitHubRepo)];
        public static string GitHubOwner => ConfigurationManager.AppSettings[nameof(GitHubOwner)];
        public static string MyName => ConfigurationManager.AppSettings[nameof(MyName)];
        public static string StackOverflowUserNumber => ConfigurationManager.AppSettings[nameof(StackOverflowUserNumber)];
        public static int OutputCacheSeconds => int.Parse(ConfigurationManager.AppSettings[nameof(OutputCacheSeconds)]);
        public static string GitHubOAuthToken => ConfigurationManager.AppSettings[nameof(GitHubOAuthToken)];
        public static string[] Quotes => ConfigurationManager.AppSettings[nameof(Quotes)].Split('|');
        public static string BlogFilesDir => ConfigurationManager.AppSettings[nameof(BlogFilesDir)];
        public static string AzureSearchIndexName => ConfigurationManager.AppSettings[nameof(AzureSearchIndexName)];
        public static string AzureSearchApiKey => ConfigurationManager.AppSettings[nameof(AzureSearchApiKey)];
        public static string AzureSearchService => ConfigurationManager.AppSettings[nameof(AzureSearchService)];
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