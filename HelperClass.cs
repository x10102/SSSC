using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SmallSimpleSerialConsole
{
    enum DataFormat
    {
        Binary = 1,
        Hex = 2,
        Octal = 3,
        Integer = 4,
        String = 5
    }
    class HelperClass
    {
        private static Regex hex = new Regex("^[ABCDEF0123456789]{2}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static bool IsHexByte(string input)
        {
            return hex.IsMatch(input);
        }
    }
}
