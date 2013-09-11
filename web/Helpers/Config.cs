using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thyme.Web.Helpers
{
    public static class Config
    {
        public static int NumPostsFrontPage
        {
            get
            {
                try
                {
                    int numPosts = int.Parse(System.Configuration.ConfigurationManager.AppSettings["FrontPageShowNumberPosts"].ToString());
                    return (numPosts>0)?numPosts:int.MaxValue;
                }
                catch (Exception) { return 0; }
            }
        }

        public static string DateTimeFormat
        {
            get {
                  try
                {return System.Configuration.ConfigurationManager.AppSettings["DateTimeFormat"].ToString();
                }
                  catch (Exception) { return "MMM dd YYYY"; }
            }
        }
    }
}