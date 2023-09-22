using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public static class StringExtension
    {
        public static bool EqualsIgnoreCase(this string a, string b)
        {
            return a.Equals(b, StringComparison.OrdinalIgnoreCase);
        }

        public static bool ContainIgnoreCase(this string str, string value)
        {
            if (str == null)
            {
                return false;
            }

            return str.Contains(value, StringComparison.OrdinalIgnoreCase);
        }

        public static string ReplaceIgnoreCase(this string str, string value, string replace)
        {
            if (str == null)
            {
                return null;
            }

            return str.Replace(value, replace);
        }
    }
}
