using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thyme.Web
{
    public static class Strings
    {
        public static string FormatWith(this string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");

            return string.Format(format, args);
        }
 

        public static bool HasValue(this string input)
        {
            return string.IsNullOrEmpty(input) == false;
        }
    }
}