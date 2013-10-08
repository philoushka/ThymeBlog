using System;
using System.Text;

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

        private static char[] Whitespace = new char[] { ' ', '\t', '\n', '\r' };
        private const char SlugSeparator = '-';
        
        public static string RemoveSlugSeparators(this string input)
        {
            return input.Replace(SlugSeparator, ' ');
        }

        public static string[] SplitNoEmpties(this string input)
        {
            return input.Split(Whitespace, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string CreateSlug(this string urlToEncode)
        {
            urlToEncode = (urlToEncode ?? "").Trim().ToLower();

            var url = new StringBuilder();

            foreach (char ch in urlToEncode)
            {
                switch (ch)
                {
                    case ' ':
                        url.Append(SlugSeparator);
                        break;
                    case '&':
                        url.Append("and");
                        break;
                    default:
                        if ((ch >= '0' && ch <= '9') ||
                            (ch >= 'a' && ch <= 'z'))
                        {
                            url.Append(ch);
                        }
                        break;
                }
            }

            return url.ToString();
        }


    }
}