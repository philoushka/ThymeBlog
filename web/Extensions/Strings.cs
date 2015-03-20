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

        public static bool IsNullorEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        public static bool HasValue(this string input)
        {
            return string.IsNullOrEmpty(input) == false;
        }

        public static bool Contains(this string input, string substringToFind, StringComparison comparisonType)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            return input.IndexOf(substringToFind, comparisonType) >= 0;
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

        /// <summary>
        /// Remove a given string from the start of this string.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="trimChars">The string to be removed from the start.</param>
        public static string TrimStart(this string target, string trimChars)
        {
            return target.TrimStart(trimChars.ToCharArray());
        }
        
        /// <summary>
        /// Remove a given string from the end of this string.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="trimChars">The string to be removed from the end.</param>
        public static string TrimEnd(this string target, string trimChars)
        {
            return target.TrimEnd(trimChars.ToCharArray());
        }


    }
}