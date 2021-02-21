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
        Hexadecimal = 2,
        Integer = 3,
        String = 4
    }
    class HelperClass
    {
        private static Regex hex = new Regex("^[ABCDEF0123456789]{2}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static bool IsHexByte(string input)
        {
            return hex.IsMatch(input);
        }

        public static bool isBinaryByte(string input)
        {
            if(input.Length == 8)
            {
                 foreach(char c in input)
                {
                    if(c != '0' && c != '1')
                    {
                        return false;
                    }
                }
                return true;
            } else
            {
                return false;
            }
        }
    }
}
