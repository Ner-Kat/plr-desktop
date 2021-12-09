using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlrDesktop.Lib
{
    public static class ApiUtils
    {
        public static string ColorDecToHex(int color)
        {
            char[] result = "000000".ToCharArray();

            string value = color.ToString("X");
            for (int i = value.Length - 1; i >= 0; i--)
                result[i] = value[i];

            return "#FF" + new string(result);
        }

        public static int ColorHexToDec(string color)
        {
            char[] fColor = "000000FF".ToCharArray();
            var rColor = color.Reverse().ToArray();
            for (int i = 0; i < rColor.Length; i++)
                fColor[i] = color[i];
            fColor = fColor.Reverse().ToArray()[2..];

            return Convert.ToInt32(new String(fColor), 16);
        }

        public static DateTime? StrToDate(string d)
        {
            if (d.StartsWith('-'))
                d = d[1..];
            int separatorInd = d.IndexOf('-');
            if (separatorInd == -1)
                return null;

            string format = "-MM-dd";
            format = format.PadLeft(format.Length + separatorInd, 'y');
            return DateTime.ParseExact(d, format, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
