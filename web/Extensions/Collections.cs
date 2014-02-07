using System.Collections.Generic;
using System.Linq;

namespace Thyme.Web
{
    public static class Collections
    {
        public static bool IsEmpty<T>(this IEnumerable<T> input)
        {
            return (input.Any() == false);
        }

    }
}