using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace Thyme.Web.Models
{
    public static class CacheState
    {
        private const string RepoRefreshDate = "RepoRefreshDate";
        public static DateTime? LastRepoRefreshDate{
            get{
                DateTime? lastRefresh=null;
                if (HttpRuntime.Cache[RepoRefreshDate] != null)
                {
                    lastRefresh = new DateTime(Convert.ToInt64(HttpRuntime.Cache[RepoRefreshDate]));
                }
                
                return lastRefresh; 
            }
            set { HttpRuntime.Cache[RepoRefreshDate] = value.Value.Ticks; }
        }
    }
}