using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thyme.Web
{
    public static class DateTimeExtensions
    {
        public static string PrettyFormat(this DateTime date )
        {
            return date.ToString(Helpers.Config.DateTimeFormat);
        }

        public static string PrettyFormat(this DateTime? date)
        {
            if (date.HasValue)
                return date.Value.PrettyFormat();
            else
                return string.Empty;
        }
    }
}