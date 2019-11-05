using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectStatus.Helpers
{
    public static class StringExtensions
    {
        public static string FixedTo(this string value, int numberOfChars)
        {
            if (value.Length <= numberOfChars)
                return value.PadRight(numberOfChars);
            else
                return value.Substring(0, numberOfChars - 3) + "...";
        }
    }
}
