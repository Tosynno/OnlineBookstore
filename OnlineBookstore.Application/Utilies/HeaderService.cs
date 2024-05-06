using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstore.Application.Utilies
{
    public static class HeaderService
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count <= 0;
        }

        //https://stackoverflow.com/questions/5284591/how-to-remove-a-suffix-from-end-of-string
        public static string RemovePostFix(this string str, params string[] postFixes)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            if (postFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var postFix in postFixes)
            {
                if (str.EndsWith(postFix))
                {
                    return str.PadLeft(str.Length - postFix.Length);
                }
            }

            return str;
        }
    }
}
